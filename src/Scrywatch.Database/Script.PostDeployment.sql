IF NOT EXISTS (SELECT 1 FROM [Merged])
BEGIN
	INSERT INTO [Merged] VALUES ('2022-01-01')
END

IF NOT EXISTS (SELECT 1 FROM [Finish])
BEGIN
	INSERT INTO [Finish]
	VALUES (1, 'nonfoil'),
		   (2, 'foil'),
		   (3, 'etched'),
		   (4, 'glossy');
END

IF NOT EXISTS (SELECT 1 FROM [Currency])
BEGIN
	INSERT INTO [Currency]
	VALUES (1, 'usd'),
		   (2, 'eur'),
		   (3, 'tix');
END

IF NOT EXISTS (SELECT 1 FROM [Face])
BEGIN
	INSERT INTO [Face]
	VALUES (1, 'front'),
		   (2, 'back');
END

IF NOT EXISTS (SELECT 1 FROM [Intention])
BEGIN
	INSERT INTO [Intention]
	VALUES (1, 'buy'),
		   (2, 'sell');
END

IF NOT EXISTS (SELECT 1 FROM [Rarity])
BEGIN
	INSERT INTO [Rarity]
	VALUES (1, 'common'),
		   (2, 'uncommon'),
		   (3, 'rare'),
		   (4, 'mythic');
END
