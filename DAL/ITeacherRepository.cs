using System;
using System.Collections.Generic;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAL
{
    public interface ITeacherRepository : IDisposable
    {
        IEnumerable<Teacher> GetTeachers();
        Teacher GetTeacherByID(int teacherId);
        void InsertTeacher(Teacher teacher);
        void DeleteTeacher(int teacherID);
        void UpdateTeacher(Teacher teacher);
        void Save();
    }
}
