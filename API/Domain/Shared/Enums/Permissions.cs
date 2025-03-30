namespace Domain.Shared.Enums;

public enum Permissions
{
	#region Basket

	BasketAccess = 10,

	#endregion

	#region Order

	OrderAccess = 20,
	OrderModify = 21,

	#endregion

	#region Product

	ProductAccess = 30,
	ProductModify = 31,
	ProductDelete = 32,

	#endregion

	#region Payment

	PaymentAccess = 40,
	PaymentModify = 41,

	#endregion

	#region User

	AccessUser = 5,
	UserAccess = 50,
	UserModify = 51,
	UserDelete = 52,

	#endregion
}

//public const string BasketAccess = "BasketAccess";