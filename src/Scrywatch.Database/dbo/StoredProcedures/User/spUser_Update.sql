CREATE PROCEDURE [dbo].[spUser_Update]
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
	UPDATE [dbo].[User]
	SET UserName = @UserName,
		NormalizedUserName = @NormalizedUserName,
		Email = @Email,
		NormalizedEmail = @NormalizedEmail,
		EmailConfirmed = @EmailConfirmed,
		PasswordHash = @PasswordHash,
		SecurityStamp = @SecurityStamp,
		ConcurrencyStamp = @ConcurrencyStamp,
		PhoneNumber = @PhoneNumber,
		PhoneNumberConfirmed = @PhoneNumberConfirmed,
		TwoFactorEnabled = @TwoFactorEnabled,
		LockoutEnd = @LockoutEnd,
		LockoutEnabled = @LockoutEnabled,
		AccessFailedCount = @AccessFailedCount
	WHERE Id = @Id
END
