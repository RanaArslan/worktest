using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAL
{
    public class EmpRepository : IEmpRepository, IDisposable
    {
        private SchoolContext context;

        public EmpRepository(SchoolContext context)
        {
            this.context = context;
        }

        public IEnumerable<Emp> GetEmps()
        {
            return context.Emps.ToList();
        }

        public Emp GetEmpByID(int id)
        {
            return context.Emps.Find(id);
        }

        public void InsertEmp(Emp emp)
        {
            context.Emps.Add(emp);
        }

        public void DeleteEmp(int empID)
        {
            Emp emp = context.Emps.Find(empID);
            context.Emps.Remove(emp);
        }

        public void UpdateEmp(Emp emp)
        {
            context.Entry(emp).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
