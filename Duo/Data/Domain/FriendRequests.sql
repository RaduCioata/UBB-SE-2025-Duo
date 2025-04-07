

DROP TABLE IF EXISTS FriendRequests;
GO

CREATE TABLE FriendRequests (
    RequestId INT IDENTITY(1,1) PRIMARY KEY,
    SenderId INT NOT NULL,
    ReceiverId INT NOT NULL,
    RequestDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (SenderId) REFERENCES Users(UserId) ON DELETE NO ACTION,
    FOREIGN KEY (ReceiverId) REFERENCES Users(UserId) ON DELETE NO ACTION,
    CHECK (SenderId <> ReceiverId) -- Prevent self-requests
);
GO





INSERT INTO FriendRequests (SenderId, ReceiverId, RequestDate)
VALUES
-- Pending friend requests
(1, 2, GETDATE()), -- Alice → Bob (Pending)
(3, 4,  GETDATE()), -- Charlie → David (Pending)

-- Accepted friend requests
(2, 3, GETDATE()), -- Bob → Charlie (Accepted)
(4, 5, GETDATE()), -- David → Emma (Accepted)
(5, 6, GETDATE()); -- Emma → Frank (Accepted)



SELECT * FROM FriendRequests;
