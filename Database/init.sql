CREATE TABLE [Coaches] (
	[id] int IDENTITY(1,1) NOT NULL UNIQUE,
	[Name] nvarchar(50) NOT NULL,
	[Email] nvarchar(50) NOT NULL,
	[PasswordHash] nvarchar(100) NOT NULL,
	PRIMARY KEY ([id])
);

CREATE TABLE [Classes] (
	[id] int IDENTITY(1,1) NOT NULL UNIQUE,
	[Name] nvarchar(50) NOT NULL,
	[CoachID] int NOT NULL,
	[TimeSlot] datetime2(7) NOT NULL,
	PRIMARY KEY ([id])
);

CREATE TABLE [Bookings] (
	[id] int IDENTITY(1,1) NOT NULL UNIQUE,
	[CoachID] int NOT NULL,
	[ClassID] int NOT NULL,
	[ClientName] nvarchar(50) NOT NULL,
	[Status] nvarchar(max) NOT NULL,
	PRIMARY KEY ([id])
);


ALTER TABLE [Classes] ADD CONSTRAINT [Classes_fk2] FOREIGN KEY ([CoachID]) REFERENCES [Coaches]([id]);
ALTER TABLE [Bookings] ADD CONSTRAINT [Bookings_fk1] FOREIGN KEY ([CoachID]) REFERENCES [Coaches]([id]);

ALTER TABLE [Bookings] ADD CONSTRAINT [Bookings_fk2] FOREIGN KEY ([ClassID]) REFERENCES [Classes]([id]);

-- Coaches
INSERT INTO [Coaches] ([Name], [Email], [PasswordHash])
VALUES 
  (N'Alice Trainer', N'alice.trainer@example.com', N'hashed_pw_1'),
  (N'Bob Coach',     N'bob.coach@example.com',     N'hashed_pw_2');

-- Classes (зв’язуємо з тренерами)
INSERT INTO [Classes] ([Name], [CoachID], [TimeSlot])
VALUES
  (N'Yoga Beginners', 1, DATEADD(DAY, 1, SYSDATETIME())),
  (N'Boxing Basics',  2, DATEADD(DAY, 2, SYSDATETIME()));

-- Bookings (зв’язуємо з CoachID і ClassID)
INSERT INTO [Bookings] ([CoachID], [ClassID], [ClientName], [Status])
VALUES
  (1, 1, N'Оксана Петрова',  N'Requested'),
  (2, 2, N'Назар Коваленко', N'Confirmed');