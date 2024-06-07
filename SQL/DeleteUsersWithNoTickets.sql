delete
from Users
where Users.UserId not in (
	SELECT u.UserId
	FROM [dbo].Users [u]
	where u.UserId in (
		SELECT t.RaisedById
		FROM [dbo].Tickets t
	) 
	or
	u.UserId in (
		SELECT ta.AssignedUserId
		FROM [dbo].TicketAssignments ta
	)
)