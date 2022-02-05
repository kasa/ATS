INSERT INTO Candidates (CandidateId, Name)
VALUES (1, 'Candidate 1'),
    (2, 'Candidate 2'),
    (3, 'Candidate 3'),
    (4, 'Candidate 4'),
    (5, 'Candidate 5'),
    (6, 'Candidate 6'),
    (7, 'Candidate 7');

INSERT INTO JobOpening (JobOpeningId, PositionName, Description)
VALUES
    (1, 'Position 1', 'Name of Position 1'),
    (2, 'Position 2', 'Name of Position 2'),
    (3, 'Position 3', 'Name of Position 3'),
    (4, 'Position 4', 'Name of Position 1'),
    (5, 'Position 5', 'Name of Position 2'),
    (6, 'Position 6', 'Name of Position 3'),
    (7, 'Position 7', 'Name of Position 1'),
    (8, 'Position 8', 'Name of Position 2'),
    (9, 'Position 9', 'Name of Position 3'),
    (10, 'Position 10', 'Name of Position 10');

INSERT INTO JobApplication (JobOpeningId, CandidateId, ApplicationDate)
VALUES
    (1, 1, '2021-01-01'),
    (1, 2, '2021-01-01'),
    (2, 1, '2021-01-01'),
    (2, 2, '2021-01-01'),
    (3, 1, '2021-01-01'),
    (3, 3, '2021-01-01'),
    (4, 1, '2021-01-01');