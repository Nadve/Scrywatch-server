namespace Scrywatch.Persistence;

public static class StoredProcedure
{
    public const string GetCard = "dbo.spCard_GetById";
    public const string GetCards = "dbo.spCard_Get";
    public const string GetCardFinishes = "dbo.spCardFinish_Get";
    public const string GetCardPrices = "dbo.spCardPrice_Get";
    public const string GetAllCardPrices = "dbo.spCardPrice_GetAll";
    public const string GetCardNames = "dbo.spName_Get";

    public const string CreateUser = "dbo.spUser_Insert";
    public const string DeleteUser = "dbo.spUser_Delete";
    public const string UpdateUser = "dbo.spUser_Update";
    public const string GetUser = "dbo.spUser_Get";
    public const string FindUser = "dbo.spUser_Find";
    public const string FindUserByEmail = "dbo.spUser_FindByEmail";

    public const string GetInterest = "dbo.spInterest_Get";
    public const string CreateInterest = "dbo.spInterest_Insert";
    public const string DeleteInterest = "dbo.spInterest_Delete";
    public const string UpdateInterest = "dbo.spInterest_Update";
    public const string FindInterest = "dbo.spInterest_Find";
    public const string FindInterestById = "dbo.spInterest_FindById";

    public const string GetLastMerge = "dbo.spMerged_Get";
    public const string UpdateMerged = "dbo.spMerged_Update";

    public const string MergeCards = "dbo.spCard_Merge";
    public const string MergeSets = "dbo.spSet_Merge";
    public const string MergeCardNames = "dbo.spName_Merge";
    public const string MergeCardFinishes = "dbo.spCardFinish_Merge";
    public const string MergeCardFaces = "dbo.spCardFace_Merge";

    public const string InsertCardPrices = "dbo.spCardPrice_Insert";

    public const string MergeNotifications = "dbo.spNotification_Merge";
    public const string GetNotifications = "dbo.spNotification_Get";
}
