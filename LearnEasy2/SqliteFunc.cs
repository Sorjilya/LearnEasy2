using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace LearnEasy2
{
    internal class SqliteFunc
    {
        static private string connectionString = "Data Source=words.db;Version=3;";

        static public List<string> GetGroupNames()
        {
            var groups = new List<string>();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var checkCmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='WordGroup';", conn);
                if (checkCmd.ExecuteScalar() == null)
                    return groups;

                var cmd = new SQLiteCommand("SELECT Name FROM WordGroup ORDER BY Name", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        groups.Add(reader.GetString(0));
                }
            }

            return groups;
        }

        static public void AddGroup(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("Название группы не может быть пустым.");

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var createCmd = new SQLiteCommand(@"
            CREATE TABLE IF NOT EXISTS WordGroup (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE
            );

            CREATE TABLE IF NOT EXISTS Word (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Word TEXT NOT NULL,
                Lan TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS WordPair (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                WordFromId INTEGER NOT NULL,
                WordToId INTEGER NOT NULL,
                GroupId INTEGER NOT NULL,
                UsageCount INTEGER NOT NULL DEFAULT 0,
                CorrectCount INTEGER NOT NULL DEFAULT 0
            );", conn))
                {
                    createCmd.ExecuteNonQuery();
                }

                using (var insertCmd = new SQLiteCommand("INSERT OR IGNORE INTO WordGroup (Name) VALUES (@name);", conn))
                {
                    insertCmd.Parameters.AddWithValue("@name", groupName.Trim());
                    insertCmd.ExecuteNonQuery();
                }
            }
        }


        static public int InsertWordAndReturnId(string word, string lang)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SQLiteCommand("INSERT INTO Word (Word, Lan) VALUES (@word, @lan); SELECT last_insert_rowid();", conn);
                cmd.Parameters.AddWithValue("@word", word);
                cmd.Parameters.AddWithValue("@lan", lang);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        static public void InsertWordPair(int wordFromId, int wordToId, int groupId)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SQLiteCommand("INSERT INTO WordPair (WordFromId, WordToId, GroupId) VALUES (@from, @to, @group);", conn);
                cmd.Parameters.AddWithValue("@from", wordFromId);
                cmd.Parameters.AddWithValue("@to", wordToId);
                cmd.Parameters.AddWithValue("@group", groupId);
                cmd.ExecuteNonQuery();
            }
        }

        static public List<WordEntry> GetAllWords()
        {
            var words = new List<WordEntry>();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var checkCmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='Word';", conn);
                if (checkCmd.ExecuteScalar() == null)
                    return words;

                var cmd = new SQLiteCommand("SELECT Id, Word, Lan FROM Word ORDER BY Id", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        words.Add(new WordEntry
                        {
                            Id = reader.GetInt32(0),
                            Word = reader.GetString(1),
                            Lan = reader.GetString(2)
                        });
                    }
                }
            }

            return words;
        }

        static public List<GroupEntry> GetAllGroups()
        {
            var groups = new List<GroupEntry>();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var check = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='WordGroup';", conn);
                if (check.ExecuteScalar() == null)
                    return groups;

                var cmd = new SQLiteCommand("SELECT Id, Name FROM WordGroup ORDER BY Name", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groups.Add(new GroupEntry
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }

            return groups;
        }

        

        static public void DeleteWordById(int wordId)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // 1. Получаем все связанные слова из WordPair
                var relatedIds = new List<int>();

                var cmd = new SQLiteCommand(@"
            SELECT WordFromId, WordToId FROM WordPair
            WHERE WordFromId = @id OR WordToId = @id;
        ", conn);
                cmd.Parameters.AddWithValue("@id", wordId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id1 = reader.GetInt32(0);
                        int id2 = reader.GetInt32(1);

                        // Добавляем в список все, кроме самого wordId (чтобы избежать дублирования)
                        if (id1 != wordId) relatedIds.Add(id1);
                        if (id2 != wordId) relatedIds.Add(id2);
                    }
                }

                // 2. Удаляем все связи из WordPair
                var deletePairs = new SQLiteCommand(@"
            DELETE FROM WordPair
            WHERE WordFromId = @id OR WordToId = @id;
        ", conn);
                deletePairs.Parameters.AddWithValue("@id", wordId);
                deletePairs.ExecuteNonQuery();

                // 3. Удаляем связанное слово(а)
                foreach (int relatedId in relatedIds)
                {
                    var delRelated = new SQLiteCommand("DELETE FROM Word WHERE Id = @rid;", conn);
                    delRelated.Parameters.AddWithValue("@rid", relatedId);
                    delRelated.ExecuteNonQuery();
                }

                // 4. Удаляем само слово
                var deleteWord = new SQLiteCommand("DELETE FROM Word WHERE Id = @id;", conn);
                deleteWord.Parameters.AddWithValue("@id", wordId);
                deleteWord.ExecuteNonQuery();
            }
        }


        static public void DeleteGroupById(int groupId)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                var cmd1 = new SQLiteCommand("DELETE FROM WordPair WHERE GroupId = @groupId", conn);
                cmd1.Parameters.AddWithValue("@groupId", groupId);
                cmd1.ExecuteNonQuery();

                var cmd2 = new SQLiteCommand("DELETE FROM WordGroup WHERE Id = @groupId", conn);
                cmd2.Parameters.AddWithValue("@groupId", groupId);
                cmd2.ExecuteNonQuery();
            }
        }
        static public List<(string, string)> GetAllGroupWords(int groupId, string langFrom, string langTo)
        {
            var result = new List<(string, string)>();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SQLiteCommand(@"
            SELECT 
                CASE WHEN w1.Lan = @langFrom THEN w1.Word ELSE w2.Word END AS WordFrom,
                CASE WHEN w1.Lan = @langFrom THEN w2.Word ELSE w1.Word END AS WordTo
            FROM WordPair wp
            JOIN Word w1 ON wp.WordFromId = w1.Id
            JOIN Word w2 ON wp.WordToId = w2.Id
            WHERE wp.GroupId = @groupId
              AND (
                   (w1.Lan = @langFrom AND w2.Lan = @langTo)
                   OR
                   (w1.Lan = @langTo AND w2.Lan = @langFrom)
              );
        ", conn);

                cmd.Parameters.AddWithValue("@groupId", groupId);
                cmd.Parameters.AddWithValue("@langFrom", langFrom);
                cmd.Parameters.AddWithValue("@langTo", langTo);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string from = reader.GetString(0);
                        string to = reader.GetString(1);
                        result.Add((from, to));
                    }
                }
            }

            return result;
        }
        static public int GetWordPairCountByGroup(int groupId)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SQLiteCommand(@"
            SELECT COUNT(*) 
            FROM WordPair 
            WHERE GroupId = @groupId;
        ", conn);

                cmd.Parameters.AddWithValue("@groupId", groupId);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        static public bool AreWordsPaired(string wordA, string wordB)
        {
            using (var conn = new SQLiteConnection("Data Source=words.db;Version=3;"))
            {
                conn.Open();

                // Найдём ID слов
                var cmd = new SQLiteCommand(@"
            SELECT Id, Word FROM Word
            WHERE Word = @wordA OR Word = @wordB;", conn);
                cmd.Parameters.AddWithValue("@wordA", wordA);
                cmd.Parameters.AddWithValue("@wordB", wordB);

                int? idA = null;
                int? idB = null;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string w = reader.GetString(1);
                        if (w == wordA)
                            idA = id;
                        else if (w == wordB)
                            idB = id;
                    }
                }

                if (idA == null || idB == null)
                    return false;

                // Проверим, есть ли такая пара (в любом порядке)
                var pairCheckCmd = new SQLiteCommand(@"
            SELECT 1 FROM WordPair
            WHERE (WordFromId = @idA AND WordToId = @idB)
               OR (WordFromId = @idB AND WordToId = @idA);", conn);
                pairCheckCmd.Parameters.AddWithValue("@idA", idA.Value);
                pairCheckCmd.Parameters.AddWithValue("@idB", idB.Value);

                var result = pairCheckCmd.ExecuteScalar();
                return result != null;
            }
        }
        // Проверка и создание таблицы, если не существует
        static private void EnsureGameResultTableExists(SQLiteConnection conn)
        {
            var checkCmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='GameResult';", conn);
            var exists = checkCmd.ExecuteScalar();
            if (exists == null)
            {
                var createCmd = new SQLiteCommand(@"
            CREATE TABLE IF NOT EXISTS GameResult (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                GameName TEXT NOT NULL,
                DatePlayed TEXT NOT NULL,
                Points INTEGER NOT NULL
            );", conn);
                createCmd.ExecuteNonQuery();
            }
        }
        static public void InitGameResultTable()
        {
            using (var conn = new SQLiteConnection("Data Source=words.db;Version=3;"))
            {
                conn.Open();
                EnsureGameResultTableExists(conn);
            }
        }


        // Добавление новой записи
        static public void InsertGameResult(string gameName, int points)
        {
            using (var conn = new SQLiteConnection("Data Source=words.db;Version=3;"))
            {
                conn.Open();
                EnsureGameResultTableExists(conn);

                var cmd = new SQLiteCommand(@"
            INSERT INTO GameResult (GameName, DatePlayed, Points)
            VALUES (@gameName, @datePlayed, @points);", conn);

                cmd.Parameters.AddWithValue("@gameName", gameName);
                cmd.Parameters.AddWithValue("@datePlayed", DateTime.UtcNow.ToString("s")); // ISO
                cmd.Parameters.AddWithValue("@points", points);
                cmd.ExecuteNonQuery();
            }
        }


        // Получение всех результатов
        static public List<GameResultEntry> GetAllGameResults()
        {
            var results = new List<GameResultEntry>();

            using (var conn = new SQLiteConnection("Data Source=words.db;Version=3;"))
            {
                conn.Open();
                EnsureGameResultTableExists(conn);

                var cmd = new SQLiteCommand("SELECT Id, GameName, DatePlayed, Points FROM GameResult ORDER BY DatePlayed DESC", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new GameResultEntry
                        {
                            Id = reader.GetInt32(0),
                            GameName = reader.GetString(1),
                            DatePlayed = reader.GetString(2),
                            Points = reader.GetInt32(3)
                        });
                    }
                }
            }

            return results;
        }
        static public int GetMaxGameScore(string gameName)
        {
            using (var conn = new SQLiteConnection("Data Source=words.db;Version=3;"))
            {
                conn.Open();

                // Убедимся, что таблица существует
                var checkCmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='GameResult';", conn);
                var exists = checkCmd.ExecuteScalar();
                if (exists == null)
                    return 0;

                var cmd = new SQLiteCommand(@"
            SELECT MAX(Points) FROM GameResult
            WHERE GameName = @gameName;", conn);
                cmd.Parameters.AddWithValue("@gameName", gameName);

                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
        public static void UpdatePairUsage(string word, bool wasCorrect)
        {
            using (var conn = new SQLiteConnection("Data Source=words.db;Version=3;"))
            {
                conn.Open();

                // Получаем ID слова
                var getIdCmd = new SQLiteCommand("SELECT Id FROM Word WHERE Word = @word", conn);
                getIdCmd.Parameters.AddWithValue("@word", word);

                var wordIdObj = getIdCmd.ExecuteScalar();
                if (wordIdObj == null)
                    return; // слово не найдено

                int wordId = Convert.ToInt32(wordIdObj);

                // Находим все пары, где участвует это слово (в любом направлении)
                var getPairsCmd = new SQLiteCommand(@"
            SELECT Id FROM WordPair 
            WHERE WordFromId = @id OR WordToId = @id", conn);
                getPairsCmd.Parameters.AddWithValue("@id", wordId);

                var pairIds = new List<int>();
                using (var reader = getPairsCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pairIds.Add(reader.GetInt32(0));
                    }
                }

                // Обновляем статистику для всех найденных пар
                foreach (var pairId in pairIds)
                {
                    var updateCmd = new SQLiteCommand(@"
                UPDATE WordPair 
                SET 
                    UsageCount = UsageCount + 1,
                    CorrectCount = CorrectCount + @correct
                WHERE Id = @pairId;", conn);
                    updateCmd.Parameters.AddWithValue("@pairId", pairId);
                    updateCmd.Parameters.AddWithValue("@correct", wasCorrect ? 1 : 0);
                    updateCmd.ExecuteNonQuery();
                }
            }
        }

        public static (int usage, int correct)? GetPairStatsByWord(string word)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                var cmd = new SQLiteCommand(@"
            SELECT UsageCount, CorrectCount
            FROM WordPair
            WHERE WordFromId = (SELECT Id FROM Word WHERE Word = @word)
               OR WordToId = (SELECT Id FROM Word WHERE Word = @word)
            LIMIT 1;", conn);

                cmd.Parameters.AddWithValue("@word", word);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (reader.GetInt32(0), reader.GetInt32(1));
                    }
                }
                return null;
            }
        }
        public class WordPairStat
        {
            public string WordA { get; set; }
            public string WordB { get; set; }
            public int Correct { get; set; }
            public int Total { get; set; }
        }

        public static List<WordPairStat> GetAllWordPairsWithStats()
        {
            var list = new List<WordPairStat>();

            using (var conn = new SQLiteConnection("Data Source=words.db;Version=3;"))
            {
                conn.Open();

                var cmd = new SQLiteCommand(@"
            SELECT 
                w1.Word, w2.Word,
                wp.CorrectCount, wp.UsageCount
            FROM WordPair wp
            JOIN Word w1 ON wp.WordFromId = w1.Id
            JOIN Word w2 ON wp.WordToId = w2.Id;", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new WordPairStat
                        {
                            WordA = reader.GetString(0),
                            WordB = reader.GetString(1),
                            Correct = reader.GetInt32(2),
                            Total = reader.GetInt32(3)
                        });
                    }
                }
            }

            return list;
        }
        public static void GenerateFakeGameResults(string gameName, int daysBack = 180, int maxGamesPerDay = 5)
        {
            var rand = new Random();
            using (var conn = new SQLiteConnection("Data Source=words.db;Version=3;"))
            {
                conn.Open();

                // Убедимся, что таблица существует
                var checkCmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='GameResult';", conn);
                if (checkCmd.ExecuteScalar() == null)
                {
                    var createCmd = new SQLiteCommand(@"
                CREATE TABLE IF NOT EXISTS GameResult (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    GameName TEXT NOT NULL,
                    DatePlayed TEXT NOT NULL,
                    Points INTEGER NOT NULL
                );", conn);
                    createCmd.ExecuteNonQuery();
                }

                var baseDate = DateTime.UtcNow.AddDays(-daysBack);

                for (int i = 0; i < daysBack; i++)
                {
                    int gamesToday = rand.Next(0, maxGamesPerDay + 1);
                    for (int j = 0; j < gamesToday; j++)
                    {
                        DateTime time = baseDate.AddDays(i).AddHours(rand.Next(0, 24)).AddMinutes(rand.Next(0, 60));

                        // Симулируем "прогресс" со временем
                        int baseScore = 10 + (int)(i * 3); // пользователь постепенно становится лучше
                        int noise = rand.Next(-30, 30);
                        int points = Math.Max(0, Math.Min(700, baseScore + noise)); // баллы в диапазоне 0–700

                        var insertCmd = new SQLiteCommand(@"
                    INSERT INTO GameResult (GameName, DatePlayed, Points)
                    VALUES (@gameName, @datePlayed, @points);", conn);

                        insertCmd.Parameters.AddWithValue("@gameName", gameName);
                        insertCmd.Parameters.AddWithValue("@datePlayed", time.ToString("s"));
                        insertCmd.Parameters.AddWithValue("@points", points);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }


    }
}
