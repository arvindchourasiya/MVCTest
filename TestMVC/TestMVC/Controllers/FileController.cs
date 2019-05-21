using TestMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestMVC.Controllers
{
    public class FileController : Controller
    {
       
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }


        [HttpPost]
        public JsonResult UploadHomeReport(string id)
        {
            List<Employee> emplList = new List<Employee>();
            List<Department> depList = new List<Department>();
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        var stream = new StreamReader(fileContent.InputStream);
                        using (stream)
                        {
                            List<string> dept = new List<string>();
                            while (stream.Peek() >= 0)
                            {
                                var line = stream.ReadLine();
                                var emp = new Employee();
                                emp.id = Convert.ToInt16(line.Split(',')[0]);
                                emp.name = line.Split(',')[1];
                                emp.salary = Convert.ToInt16(line.Split(',')[2]);
                                emplList.Add(emp);
                                dept.Add(line.Split(',')[3]);
                            }
                            var unique_dept = new HashSet<string>(dept);
                            int i = 1;
                            foreach (var uDept in unique_dept)
                            {
                                depList.Add(new Department{
                                    id = i,
                                    deptName = uDept
                                });
                                i++;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Json("Please check file content try again");
            }
            EmpDepViewModel empDepViewModel = new EmpDepViewModel();
            empDepViewModel.emplList = emplList;
            empDepViewModel.deptList = depList;
            return Json(empDepViewModel);
        }
    }
}