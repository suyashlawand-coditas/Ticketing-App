delete from Users 
where Users.UserId in (select Users.UserId
from Users 
join UserRoles ur on Users.UserId = ur.UserId
where Users.UserId not in (
	select distinct t.RaisedById
	from Tickets t
) and ur.Role = 1);