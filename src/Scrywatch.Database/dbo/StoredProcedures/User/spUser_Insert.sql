CREATE PROCEDURE [dbo].[spUser_Insert]
	@Id nvarchar(450),
	@UserName nvarchar(256),
	@NormalizedUserName nvarchar(256),
	@Email nvarchar(256),
	@NormalizedEmail nvarchar(256),
	@EmailConfirmed bit,
	@PasswordHash nvarchar(max),
	@SecurityStamp nvarchar(max),
	@ConcurrencyStamp nvarchar(max),
	@PhoneNumber nvarchar(max),
	@PhoneNumberConfirmed bit,
	@TwoFactorEnabled bit,
	@LockoutEnd datetimeoffset(7),
	@LockoutEnabled bit,
	@AccessFailedCount int
AS
BEGIN
	INSERT INTO [dbo].[User]
	(
		Id,
		UserName,
		NormalizedUserName,
		Email,
		NormalizedEmail,
		EmailConfirmed,
		PasswordHash,
		SecurityStamp,
		ConcurrencyStamp,
		PhoneNumber,
		PhoneNumberConfirmed,
		TwoFactorEnabled,
		LockoutEnd,
		LockoutEnabled,
		AccessFailedCount
	)
	VALUES
	(
		@Id,
		@UserName,
		@NormalizedUserName,
		@Email,
		@NormalizedEmail,
		@EmailConfirmed,
		@PasswordHash,
		@SecurityStamp,
		@ConcurrencyStamp,
		@PhoneNumber,
		@PhoneNumberConfirmed,
		@TwoFactorEnabled,
		@LockoutEnd,
		@LockoutEnabled,
		@AccessFailedCount
	)
END
