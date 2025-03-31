namespace API.Controllers.Common;

using System.Globalization;

using Domain.Entities.User;
using Domain.Shared.Constants;

using Microsoft.AspNetCore.Mvc;

public class ApiBaseController : ControllerBase
{

	public User? CurrentUser { get; set; }

	public Guid? CurrentUserId { get; set; } = Guid.Empty;

	public CultureInfo CultureInfo { get; set; } = new CultureInfo(CultureInfos.English_US);

}
