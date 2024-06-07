delete 
from [dbo].Departments
where Departments.DepartmentId not in (
		select d.DepartmentId 
		from [dbo].departments [d]  
		right join [dbo].Users [u] on u.DepartmentId = d.DepartmentId
		group by [d].DepartmentId
	)