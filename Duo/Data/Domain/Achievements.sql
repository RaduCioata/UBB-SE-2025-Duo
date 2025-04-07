CREATE TABLE Achievements (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NOT NULL,
    Rarity NVARCHAR(50) NOT NULL
);

-- Insert achievements for streaks
INSERT INTO Achievements (Name, Description, Rarity) VALUES
('10 Day Streak', 'Achieve a 10-day streak', 'Common'),
('50 Day Streak', 'Achieve a 50-day streak', 'Uncommon'),
('100 Day Streak', 'Achieve a 100-day streak', 'Rare'),
('250 Day Streak', 'Achieve a 250-day streak', 'Epic'),
('500 Day Streak', 'Achieve a 500-day streak', 'Legendary'),
('1000 Day Streak', 'Achieve a 1000-day streak', 'Mythic');

-- Insert achievements for quizzes completed
INSERT INTO Achievements (Name, Description, Rarity) VALUES
('10 Quizzes Completed', 'Complete 10 quizzes', 'Common'),
('50 Quizzes Completed', 'Complete 50 quizzes', 'Uncommon'),
('100 Quizzes Completed', 'Complete 100 quizzes', 'Rare'),
('250 Quizzes Completed', 'Complete 250 quizzes', 'Epic'),
('500 Quizzes Completed', 'Complete 500 quizzes', 'Legendary'),
('1000 Quizzes Completed', 'Complete 1000 quizzes', 'Mythic');

-- Insert achievements for courses completed
INSERT INTO Achievements (Name, Description, Rarity) VALUES
('1 Course Completed', 'Complete 10 courses', 'Common'),
('5 Courses Completed', 'Complete 50 courses', 'Uncommon'),
('10 Courses Completed', 'Complete 100 courses', 'Rare'),
('25 Courses Completed', 'Complete 250 courses', 'Epic'),
('50 Courses Completed', 'Complete 500 courses', 'Legendary'),
('100 Courses Completed', 'Complete 1000 courses', 'Mythic');


select * from Achievements