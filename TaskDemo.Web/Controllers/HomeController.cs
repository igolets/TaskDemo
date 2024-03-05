using System.Linq;
using TaskDemo.Data.Common.Repository;
using TaskDemo.Data.EF;
using System.Web.Mvc;
using TaskDemo.Web.Models;

namespace TaskDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Consts

        private const string NameUniquenessMessage = "Task name should be unique on this nesting level";
        private const string DeleteInheritanceMessage = "Can not delete this task, because it contains other tasks.";

        #endregion

        #region Constructors

        public HomeController(IRepository<Task> taskRepo)
        {
            _taskRepo = taskRepo;
        }

        #endregion Constructors

        #region Public methods

        public ActionResult Index(int? id)
        {
            TaskModel[] models;
            int? parentId = null;
            if (id.HasValue)
            {
                var task = _taskRepo.GetById(id);
                if (task != null)
                {
                    parentId = task.ParentId;
                    models = task.Children.Select(x => new TaskModel { Id = x.Id, Name = x.Name }).ToArray();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                var tasks = _taskRepo.GetQ().Where(x => x.Parent == null).ToArray();
                models = tasks.Select(x => new TaskModel { Id = x.Id, Name = x.Name }).ToArray();
            }
            return View(new TaskListModel { Id = id, ParentId = parentId, Tasks = models });
        }

        [HttpGet]
        public ActionResult Create(int? parentId)
        {
            if (!parentId.HasValue)
            {
                return View(new TaskCreateModel ());
            }

            var task = _taskRepo.GetById(parentId);
            if (task != null)
            {
                return View(new TaskCreateModel { ParentId = parentId });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(TaskCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var toAdd = new Task
                {
                    Name = model.Name,
                    ParentId = model.ParentId
                };

                _taskRepo.Insert(toAdd);
                try
                {
                    _taskRepo.Save();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    var msg = ex.InnerException?.InnerException?.Message;
                    if (msg != null && msg.StartsWith("Cannot insert duplicate key row in object 'dbo.Task' with unique index 'IX_ParentId_Name'"))
                    {
                        ModelState.AddModelError("", NameUniquenessMessage);
                        return View(model);
                    }

                    throw;
                }
                return RedirectToAction("Index", new { Id = model.ParentId });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var task = _taskRepo.GetById(id);
            if (task != null)
            {
                return View(new TaskEditModel { Id = id, Name = task.Name, ParentId = task.ParentId});
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(TaskEditModel model)
        {
            if (ModelState.IsValid)
            {
                var task = _taskRepo.GetById(model.Id);
                task.Name = model.Name;

                _taskRepo.Update(task);
                try
                {
                    _taskRepo.Save();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    var msg = ex.InnerException?.InnerException?.Message;
                    if (msg != null && msg.StartsWith("Cannot insert duplicate key row in object 'dbo.Task' with unique index 'IX_ParentId_Name'"))
                    {
                        ModelState.AddModelError("", NameUniquenessMessage);
                        model.ParentId = task.ParentId;
                        return View(model);
                    }

                    throw;
                }
                return RedirectToAction("Index", new { Id = task.ParentId });
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var temp = _taskRepo.GetById(id);
            if (temp != null)
            {
                var parentId = temp.ParentId;
                if (temp.Children.Count == 0)
                {
                    _taskRepo.Delete(temp);
                    try
                    {
                        _taskRepo.Save();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        var msg = ex.InnerException?.InnerException?.Message;
                        if (msg != null && msg.StartsWith("The DELETE statement conflicted with the SAME TABLE REFERENCE constraint"))
                        {
                            return View(new TaskDeleteModel
                            {
                                Id = id,
                                ParentId = parentId,
                                Message = DeleteInheritanceMessage
                            });
                        }

                        throw;
                    }
                }
                else
                {
                    return View(new TaskDeleteModel
                    {
                        Id = id,
                        ParentId = parentId,
                        Message = DeleteInheritanceMessage
                    });
                }
                return RedirectToAction("Index", new {Id=parentId});
            }
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        #endregion

        #region Fields

        private readonly IRepository<Task> _taskRepo;

        #endregion Fields
    }
}