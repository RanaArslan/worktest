using System;
using System.Collections.Generic;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAL
{
    public interface IEmpRepository : IDisposable
    {
        IEnumerable<Emp> GetEmps();
        Emp GetEmpByID(int empId);
        void InsertEmp(Emp student);
        void DeleteEmp(int empID);
        void UpdateEmp(Emp emp);
        void Save();
    }
}
