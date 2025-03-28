﻿namespace Infrastructure.Persistence.Repositories;

using Domain.DTOs.Order;
using Domain.Entities.User;
using Domain.Interfaces.Repository;

using Microsoft.EntityFrameworkCore;

public class UserRepository(StoreContext storeContext) : IUserRepository
{
	public async Task<bool> AddUsersAddressAsync(string buyerId, CreateOrderDto orderDto)
	{
		var user = await storeContext.Users
				.Include(u => u.Address)
				.FirstOrDefaultAsync(x => x.UserName == buyerId);

		var address = new Address
		{
			FullName = orderDto.ShippingAddress.FullName,
			Address1 = orderDto.ShippingAddress.Address1,
			Address2 = orderDto.ShippingAddress.Address2,
			City = orderDto.ShippingAddress.City,
			State = orderDto.ShippingAddress.State,
			Zip = orderDto.ShippingAddress.Zip,
			Country = orderDto.ShippingAddress.Country
		};
		user.Address = address;
		storeContext.Update(user);
		return await storeContext.SaveChangesAsync() != 0;
	}
}
