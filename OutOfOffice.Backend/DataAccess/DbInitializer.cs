using DataAccess.Entities;

namespace DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(OutOfOfficeDbContext context)
        {
            context.Database.EnsureCreated();
            SeedDatabase(context);
        }

        public static void SeedDatabase(OutOfOfficeDbContext context)
        {
            if (context.Employee.Any())
            {
                return; // DB has been seeded
            }

            var employees = new Employee[]
            {
                new Employee { FullName = "John Doe", Subdivision = "HR", Position = "HR Manager", Status = "Active", PeoplePartnerId = 8, OutOfOfficeBalance = 10 },
                new Employee { FullName = "Jane Smith", Subdivision = "IT", Position = "Project Manager", Status = "Active", PeoplePartnerId = 1, OutOfOfficeBalance = 8 },
                new Employee { FullName = "Alice Johnson", Subdivision = "Marketing", Position = "Marketing Specialist", Status = "Active", PeoplePartnerId = 1, OutOfOfficeBalance = 12 },
                new Employee { FullName = "Bob Brown", Subdivision = "Finance", Position = "Accountant", Status = "Active", PeoplePartnerId = 1, OutOfOfficeBalance = 15 },
                new Employee { FullName = "Charlie Davis", Subdivision = "IT", Position = "Developer", Status = "Active", PeoplePartnerId = 1, OutOfOfficeBalance = 20 },
                new Employee { FullName = "Eva Green", Subdivision = "Sales", Position = "Sales Manager", Status = "Active", PeoplePartnerId = 1, OutOfOfficeBalance = 18 },
                new Employee { FullName = "Frank Harris", Subdivision = "Support", Position = "Support Specialist", Status = "Active", PeoplePartnerId = 1, OutOfOfficeBalance = 14 },
                new Employee { FullName = "Grace Kelly", Subdivision = "HR", Position = "HR Manager", Status = "Active", PeoplePartnerId = 1, OutOfOfficeBalance = 11 },
                new Employee { FullName = "Helen Moore", Subdivision = "IT", Position = "Developer", Status = "Active", PeoplePartnerId = 8, OutOfOfficeBalance = 13 },
                new Employee { FullName = "Ian Nelson", Subdivision = "IT", Position = "Developer", Status = "Active", PeoplePartnerId = 8, OutOfOfficeBalance = 9 },
                new Employee { FullName = "Jackie O'Conner", Subdivision = "Marketing", Position = "Marketing Specialist", Status = "Active", PeoplePartnerId = 1, OutOfOfficeBalance = 16 },
                new Employee { FullName = "Kevin Price", Subdivision = "Finance", Position = "Accountant", Status = "Active", PeoplePartnerId = 8, OutOfOfficeBalance = 7 },
                new Employee { FullName = "Laura Quinn", Subdivision = "Sales", Position = "Sales Specialist", Status = "Active", PeoplePartnerId = 1, OutOfOfficeBalance = 19 },
                new Employee { FullName = "Michael Scott", Subdivision = "IT", Position = "Project Manager", Status = "Active", PeoplePartnerId = 1, OutOfOfficeBalance = 22 },
                new Employee { FullName = "Nancy Thomas", Subdivision = "Support", Position = "Support Specialist", Status = "Active", PeoplePartnerId = 8, OutOfOfficeBalance = 17 }
            };

            context.Employee.AddRange(employees);
            context.SaveChanges();

            var leaveRequests = new LeaveRequest[]
            {
                new LeaveRequest { EmployeeId = 1, AbsenceReason = "Vacation", Comment = "Going to Hawaii", StartDate = DateTime.Now.AddDays(10), EndDate = DateTime.Now.AddDays(20), Status = "New", Employee = employees[0] },
                new LeaveRequest { EmployeeId = 2, AbsenceReason = "Sick Leave", Comment = "Flu", StartDate = DateTime.Now.AddDays(5), EndDate = DateTime.Now.AddDays(7), Status = "New", Employee = employees[1] },
                new LeaveRequest { EmployeeId = 3, AbsenceReason = "Vacation", Comment = "Visiting family", StartDate = DateTime.Now.AddDays(15), EndDate = DateTime.Now.AddDays(25), Status = "Approved", Employee = employees[2] },
                new LeaveRequest { EmployeeId = 4, AbsenceReason = "Conference", Comment = "Attending tech conference", StartDate = DateTime.Now.AddDays(20), EndDate = DateTime.Now.AddDays(22), Status = "New", Employee = employees[3] },
                new LeaveRequest { EmployeeId = 5, AbsenceReason = "Sick Leave", Comment = "Migraine", StartDate = DateTime.Now.AddDays(3), EndDate = DateTime.Now.AddDays(4), Status = "Rejected", Employee = employees[4] },
                new LeaveRequest { EmployeeId = 6, AbsenceReason = "Training", Comment = "Sales training", StartDate = DateTime.Now.AddDays(12), EndDate = DateTime.Now.AddDays(13), Status = "Approved", Employee = employees[5] },
                new LeaveRequest { EmployeeId = 7, AbsenceReason = "Personal Leave", Comment = "Personal matters", StartDate = DateTime.Now.AddDays(30), EndDate = DateTime.Now.AddDays(35), Status = "New", Employee = employees[6] },
                new LeaveRequest { EmployeeId = 8, AbsenceReason = "Vacation", Comment = "Beach holiday", StartDate = DateTime.Now.AddDays(40), EndDate = DateTime.Now.AddDays(50), Status = "New", Employee = employees[7] },
                new LeaveRequest { EmployeeId = 9, AbsenceReason = "Sick Leave", Comment = "Back pain", StartDate = DateTime.Now.AddDays(6), EndDate = DateTime.Now.AddDays(8), Status = "New", Employee = employees[8] },
                new LeaveRequest { EmployeeId = 10, AbsenceReason = "Vacation", Comment = "Mountain trip", StartDate = DateTime.Now.AddDays(15), EndDate = DateTime.Now.AddDays(20), Status = "New", Employee = employees[9] },
                new LeaveRequest { EmployeeId = 11, AbsenceReason = "Conference", Comment = "Marketing conference", StartDate = DateTime.Now.AddDays(25), EndDate = DateTime.Now.AddDays(28), Status = "New", Employee = employees[10] },
                new LeaveRequest { EmployeeId = 12, AbsenceReason = "Training", Comment = "Finance training", StartDate = DateTime.Now.AddDays(10), EndDate = DateTime.Now.AddDays(12), Status = "Approved", Employee = employees[11] },
                new LeaveRequest { EmployeeId = 13, AbsenceReason = "Vacation", Comment = "Europe trip", StartDate = DateTime.Now.AddDays(30), EndDate = DateTime.Now.AddDays(40), Status = "New", Employee = employees[12] }
            };

            context.LeaveRequest.AddRange(leaveRequests);
            context.SaveChanges();

            var approvalRequests = new ApprovalRequest[]
            {
                new ApprovalRequest { ApproverId = 1, LeaveRequestId = 1, Status = "New", Comment = "Pending approval", Approver = employees[0], LeaveRequest = leaveRequests[0] },
                new ApprovalRequest { ApproverId = 2, LeaveRequestId = 2, Status = "New", Comment = "Pending approval", Approver = employees[1], LeaveRequest = leaveRequests[1] },
                new ApprovalRequest { ApproverId = 3, LeaveRequestId = 3, Status = "Approved", Comment = "Approved", Approver = employees[2], LeaveRequest = leaveRequests[2] },
                new ApprovalRequest { ApproverId = 4, LeaveRequestId = 4, Status = "New", Comment = "Pending approval", Approver = employees[3], LeaveRequest = leaveRequests[3] },
                new ApprovalRequest { ApproverId = 5, LeaveRequestId = 5, Status = "Rejected", Comment = "Not approved", Approver = employees[4], LeaveRequest = leaveRequests[4] },
                new ApprovalRequest { ApproverId = 6, LeaveRequestId = 6, Status = "Approved", Comment = "Approved", Approver = employees[5], LeaveRequest = leaveRequests[5] },
                new ApprovalRequest { ApproverId = 7, LeaveRequestId = 7, Status = "New", Comment = "Pending approval", Approver = employees[6], LeaveRequest = leaveRequests[6] },
                new ApprovalRequest { ApproverId = 8, LeaveRequestId = 8, Status = "New", Comment = "Pending approval", Approver = employees[7], LeaveRequest = leaveRequests[7] },
                new ApprovalRequest { ApproverId = 9, LeaveRequestId = 9, Status = "New", Comment = "Pending approval", Approver = employees[8], LeaveRequest = leaveRequests[8] },
                new ApprovalRequest { ApproverId = 10, LeaveRequestId = 10, Status = "New", Comment = "Pending approval", Approver = employees[9], LeaveRequest = leaveRequests[9] }
            };

            context.ApprovalRequest.AddRange(approvalRequests);
            context.SaveChanges();

            var projects = new Project[]
            {
                new Project { ProjectType = "Software Development", StartDate = DateTime.Now.AddDays(-30), EndDate = DateTime.Now.AddDays(60), ProjectManagerId = 2, Comment = "New software project", Status = "Active", ProjectManager = employees[1] },
                new Project { ProjectType = "Marketing Campaign", StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now.AddDays(50), ProjectManagerId = 3, Comment = "Launch new campaign", Status = "Active", ProjectManager = employees[2] },
                new Project { ProjectType = "Finance Analysis", StartDate = DateTime.Now.AddDays(-20), EndDate = DateTime.Now.AddDays(70), ProjectManagerId = 4, Comment = "Financial analysis project", Status = "Active", ProjectManager = employees[3] },
                new Project { ProjectType = "IT Infrastructure", StartDate = DateTime.Now.AddDays(-15), EndDate = DateTime.Now.AddDays(45), ProjectManagerId = 5, Comment = "Infrastructure upgrade", Status = "Active", ProjectManager = employees[4] },
                new Project { ProjectType = "Sales Strategy", StartDate = DateTime.Now.AddDays(-25), EndDate = DateTime.Now.AddDays(35), ProjectManagerId = 6, Comment = "New sales strategy", Status = "Active", ProjectManager = employees[5] },
                new Project { ProjectType = "Customer Support Improvement", StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now.AddDays(40), ProjectManagerId = 7, Comment = "Improve support", Status = "Active", ProjectManager = employees[6] },
                new Project { ProjectType = "HR Policy Revision", StartDate = DateTime.Now.AddDays(-5), EndDate = DateTime.Now.AddDays(30), ProjectManagerId = 8, Comment = "Revise HR policies", Status = "Active", ProjectManager = employees[7] },
                new Project { ProjectType = "Marketing Research", StartDate = DateTime.Now.AddDays(-20), EndDate = DateTime.Now.AddDays(60), ProjectManagerId = 9, Comment = "Conduct marketing research", Status = "Active", ProjectManager = employees[8] },
                new Project { ProjectType = "Product Development", StartDate = DateTime.Now.AddDays(-15), EndDate = DateTime.Now.AddDays(75), ProjectManagerId = 10, Comment = "Develop new product", Status = "Active", ProjectManager = employees[9] },
                new Project { ProjectType = "Regional Expansion", StartDate = DateTime.Now.AddDays(-30), EndDate = DateTime.Now.AddDays(90), ProjectManagerId = 11, Comment = "Expand regionally", Status = "Active", ProjectManager = employees[10] }
            };

            context.Project.AddRange(projects);
            context.SaveChanges();
        }
    }
}
