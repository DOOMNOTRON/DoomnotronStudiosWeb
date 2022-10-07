Select Name, Username, Email
From AspNetUsers 
	Join AspNetUserRoles ON AspNetUsers.Id = UserId
	Join AspNetRoles ON RoleId = AspNetRoles.Id

	Select Name, Username, Email
From AspNetUsers 
	Left Join AspNetUserRoles ON AspNetUsers.Id = UserId
	Left Join AspNetRoles ON RoleId = AspNetRoles.Id