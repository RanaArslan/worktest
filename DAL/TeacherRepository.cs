using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAL
{
    public class TeacherRepository : ITeacherRepository, IDisposable
    {
        private SchoolContext context;

        public TeacherRepository(SchoolContext context)
        {
            this.context = context;
        }

        public IEnumerable<Teacher> GetTeachers()
        {
            return context.Teachers.ToList();
        }

        public Teacher GetTeacherByID(int id)
        {
            return context.Teachers.Find(id);
        }

        public void InsertTeacher(Teacher teacher)
        {
            context.Teachers.Add(teacher);
        }

        public void DeleteTeacher(int teacherID)
        {
            Teacher teacher = context.Teachers.Find(teacherID);
            context.Teachers.Remove(teacher);
        }

        public void UpdateTeacher(Teacher teacher)
        {
            context.Entry(teacher).State = EntityState.Modified;
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
