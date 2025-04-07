DROP TABLE IF EXISTS Friends;
GO

CREATE TABLE Friends (
    FriendshipId INT IDENTITY(1,1) PRIMARY KEY,
    UserId1 INT NOT NULL,
    UserId2 INT NOT NULL,
    FOREIGN KEY (UserId1) REFERENCES Users(UserId) ON DELETE NO ACTION,
    FOREIGN KEY (UserId2) REFERENCES Users(UserId) ON DELETE NO ACTION,
    CHECK (UserId1 <> UserId2) -- Prevent self-friendship
);
GO


INSERT INTO Friends (UserId1, UserId2)
VALUES 
    (2, 3),  -- Bob and Charlie
    (4, 5),  -- David and Emma
    (5, 6);  -- Emma and Frank

INSERT INTO Friends (UserId1, UserId2)
VALUES 
    (1, 6),  -- Bob and Charlie
    (1, 5),  -- David and Emma
    (1, 2),  -- Emma and Frank
	(1,4);

SELECT * FROM Friends;
