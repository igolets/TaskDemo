using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using TaskDemo.Data.Common.Repository;
using TaskDemo.Data.EF;
using TaskDemo.Web.Controllers;
using TaskDemo.Web.Models;

namespace TaskDemo.Web.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index_NoTasks()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var empty = new Task[0];
            taskRepoMock.Setup(r => r.GetQ(null)).Returns(empty.AsQueryable());
            var ctrl = new HomeController(taskRepoMock.Object);

            // Act
            var rez = ctrl.Index(null);

            // Assert
            taskRepoMock.VerifyAll();
            rez.ShouldBeOfType<ViewResult>();
            var modelTemp = ((ViewResult)rez).Model;
            modelTemp.ShouldNotBeNull();
            modelTemp.ShouldBeOfType<TaskListModel>();
            var listModel = (TaskListModel)modelTemp;
            listModel.ParentId.ShouldBe(null);
            listModel.Id.ShouldBe(null);
            listModel.Tasks.ShouldBeEmpty();
        }

        [TestMethod]
        public void Index_Root()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var testParent = new Task {Id = 3, ParentId = null, Name = "a3"};
            var testData = new []
            {
                new Task{Id = 1, ParentId = null, Name = "a1"},
                new Task{Id = 2, ParentId = null, Name = "a2"},
                testParent,
                new Task{Id = 4, ParentId = testParent.Id, Parent = testParent, Name = "b4"}
            };
            taskRepoMock.Setup(r => r.GetQ(null)).Returns(testData.AsQueryable());
            var ctrl = new HomeController(taskRepoMock.Object);

            // Act
            var rez = ctrl.Index(null);

            // Assert
            taskRepoMock.VerifyAll();
            rez.ShouldBeOfType<ViewResult>();
            var modelTemp = ((ViewResult)rez).Model;
            modelTemp.ShouldNotBeNull();
            modelTemp.ShouldBeOfType<TaskListModel>();
            var listModel = (TaskListModel)modelTemp;
            listModel.ParentId.ShouldBe(null);
            listModel.Id.ShouldBe(null);
            listModel.Tasks.Length.ShouldBe(3);

            var model = listModel.Tasks.SingleOrDefault(x => x.Id == 1);
            model.ShouldNotBeNull();
            model.Name.ShouldBe("a1");

            model = listModel.Tasks.SingleOrDefault(x => x.Id == 2);
            model.ShouldNotBeNull();
            model.Name.ShouldBe("a2");

            model = listModel.Tasks.SingleOrDefault(x => x.Id == 3);
            model.ShouldNotBeNull();
            model.Name.ShouldBe("a3");
        }

        [TestMethod]
        public void Index_ParentNotExist()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            taskRepoMock.Setup(r => r.GetByID(5)).Returns((Task)null);
            var ctrl = new HomeController(taskRepoMock.Object);

            // Act
            var rez = ctrl.Index(5);

            // Assert
            taskRepoMock.VerifyAll();
            rez.ShouldBeOfType<RedirectToRouteResult>();
            var routeResult = (RedirectToRouteResult)rez;
            routeResult.RouteValues.Count.ShouldBe(1);
            routeResult.RouteValues["action"].ShouldBe("Index");
        }

        [TestMethod]
        public void Index_OneLevelDown()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var testParent3 = new Task { Id = 3, ParentId = null, Name = "a3" };
            var testParent2 = new Task { Id = 2, ParentId = null, Name = "a2" };
            var testData = new[]
            {
                new Task{Id = 1, ParentId = null, Name = "a1"},
                testParent2,
                testParent3,
                new Task{Id = 4, ParentId = testParent3.Id, Parent = testParent3, Name = "b4"},
                new Task{Id = 5, ParentId = testParent2.Id, Parent = testParent2, Name = "c4"}
            };
            testParent3.Children = testData.Where(t => t.ParentId == testParent3.Id).ToArray();
            taskRepoMock.Setup(r => r.GetByID(3)).Returns(testParent3);
            var ctrl = new HomeController(taskRepoMock.Object);

            // Act
            var rez = ctrl.Index(3);

            // Assert
            taskRepoMock.VerifyAll();
            rez.ShouldBeOfType<ViewResult>();
            var modelTemp = ((ViewResult)rez).Model;
            modelTemp.ShouldNotBeNull();
            modelTemp.ShouldBeOfType<TaskListModel>();
            var listModel = (TaskListModel)modelTemp;
            listModel.ParentId.ShouldBe(null);
            listModel.Id.ShouldBe(3);
            listModel.Tasks.Length.ShouldBe(1);

            var model = listModel.Tasks.SingleOrDefault(x => x.Id == 4);
            model.ShouldNotBeNull();
            model.Name.ShouldBe("b4");
        }

        [TestMethod]
        public void Index_TwoLevelsDown()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var testParent2 = new Task { Id = 2, ParentId = null, Name = "a2" };
            var testParent3 = new Task { Id = 3, ParentId = testParent2.Id, Parent = testParent2, Name = "a3" };
            var testData = new[]
            {
                new Task{Id = 1, ParentId = null, Name = "a1"},
                testParent2,
                testParent3,
                new Task{Id = 4, ParentId = testParent3.Id, Parent = testParent3, Name = "b4"},
                new Task{Id = 5, ParentId = testParent2.Id, Parent = testParent2, Name = "c4"}
            };
            testParent3.Children = testData.Where(t => t.ParentId == testParent3.Id).ToArray();
            taskRepoMock.Setup(r => r.GetByID(3)).Returns(testParent3);
            var ctrl = new HomeController(taskRepoMock.Object);

            // Act
            var rez = ctrl.Index(3);

            // Assert
            taskRepoMock.VerifyAll();
            rez.ShouldBeOfType<ViewResult>();
            var modelTemp = ((ViewResult)rez).Model;
            modelTemp.ShouldNotBeNull();
            modelTemp.ShouldBeOfType<TaskListModel>();
            var listModel = (TaskListModel)modelTemp;
            listModel.ParentId.ShouldBe(2);
            listModel.Id.ShouldBe(3);
            listModel.Tasks.Length.ShouldBe(1);

            var model = listModel.Tasks.SingleOrDefault(x => x.Id == 4);
            model.ShouldNotBeNull();
            model.Name.ShouldBe("b4");
        }

        [TestMethod]
        public void Create_Get_Root()
        {
            // Arrange
            var controller = new HomeController(null);

            // Act
            var result = controller.Create((int?)null) as ViewResult;

            // Assert
            result.ShouldNotBeNull();
            result.Model.ShouldBeOfType<TaskCreateModel>();
            var model = (TaskCreateModel)result.Model;
            model.ParentId.ShouldBeNull();
        }

        [TestMethod]
        public void Create_Get_NotRoot()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            taskRepoMock.Setup(r => r.GetByID(1)).Returns(new Task());
            var controller = new HomeController(taskRepoMock.Object);

            // Act
            var result = controller.Create(1) as ViewResult;

            // Assert
            taskRepoMock.VerifyAll();
            result.ShouldNotBeNull();
            result.Model.ShouldBeOfType<TaskCreateModel>();
            var model = (TaskCreateModel)result.Model;
            model.ParentId.ShouldBe(1);
        }

        [TestMethod]
        public void Create_Get_NotRootNotExists()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            taskRepoMock.Setup(r => r.GetByID(1)).Returns((Task)null);
            var controller = new HomeController(taskRepoMock.Object);

            // Act
            var rez = controller.Create(1);

            // Assert
            taskRepoMock.VerifyAll();
            rez.ShouldBeOfType<RedirectToRouteResult>();
            var routeResult = (RedirectToRouteResult)rez;
            routeResult.RouteValues.Count.ShouldBe(1);
            routeResult.RouteValues["action"].ShouldBe("Index");
        }

        [TestMethod]
        public void Create_Post_ValidationFailed()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var inModel = new TaskCreateModel {ParentId = null, Name = null};
            controller.ModelState.AddModelError("","");
            taskRepoMock.Verify(r => r.Save(), Times.Never);

            // Act
            var rez = controller.Create(inModel);

            // Assert
            rez.ShouldBeOfType<ViewResult>();
            var modelTemp = ((ViewResult)rez).Model;
            modelTemp.ShouldNotBeNull();
            modelTemp.ShouldBeOfType<TaskCreateModel>();
            var model = (TaskCreateModel)modelTemp;
            model.ParentId.ShouldBe(null);
            model.Name.ShouldBe(null);
        }

        [TestMethod]
        public void Create_Post_Success()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var inModel = new TaskCreateModel { ParentId = 1, Name = "a1" };
            Task callbackTask = null;
            taskRepoMock.Setup(r => r.Insert(It.IsAny<Task>())).Callback((Task t) => { callbackTask = t; });
            taskRepoMock.Setup(r=>r.Save()).Verifiable();

            // Act
            var rez = controller.Create(inModel);

            // Assert
            taskRepoMock.VerifyAll();
            callbackTask.ShouldNotBeNull();
            callbackTask.ParentId.ShouldBe(1);
            callbackTask.Name.ShouldBe("a1");

            rez.ShouldBeOfType<RedirectToRouteResult>();
            var routeResult = (RedirectToRouteResult)rez;
            routeResult.RouteValues.Count.ShouldBe(2);
            routeResult.RouteValues["action"].ShouldBe("Index");
            routeResult.RouteValues["id"].ShouldBe(1);
        }

        [TestMethod]
        public void Create_Post_ErrorHandled()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var inModel = new TaskCreateModel { ParentId = 1, Name = "a1" };
            Task callbackTask = null;
            taskRepoMock.Setup(r => r.Insert(It.IsAny<Task>())).Callback((Task t) => { callbackTask = t; });
            var ex = new Exception("Cannot insert duplicate key row in object 'dbo.Task' with unique index 'IX_ParentId_Name'");
            var dbEx = new System.Data.Entity.Infrastructure.DbUpdateException("",new Exception("",ex));
            taskRepoMock.Setup(r => r.Save()).Throws(dbEx);

            // Act
            var rez = controller.Create(inModel);

            // Assert
            taskRepoMock.VerifyAll();
            callbackTask.ShouldNotBeNull();
            callbackTask.ParentId.ShouldBe(1);
            callbackTask.Name.ShouldBe("a1");
            controller.ModelState.Count.ShouldBe(1);
            controller.ModelState[""].Errors.Single().ErrorMessage.ShouldBe("Task name should be unique on this nesting level");

            rez.ShouldBeOfType<ViewResult>();
            var modelTemp = ((ViewResult)rez).Model;
            modelTemp.ShouldNotBeNull();
            modelTemp.ShouldBeOfType<TaskCreateModel>();
            var model = (TaskCreateModel)modelTemp;
            model.ParentId.ShouldBe(1);
            model.Name.ShouldBe("a1");
        }

        [TestMethod]
        public void Create_Post_ErrorNotHandledText()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var inModel = new TaskCreateModel { ParentId = 1, Name = "a1" };
            Task callbackTask = null;
            taskRepoMock.Setup(r => r.Insert(It.IsAny<Task>())).Callback((Task t) => { callbackTask = t; });
            var ex = new Exception("Wrong exception message");
            var dbEx = new System.Data.Entity.Infrastructure.DbUpdateException("", new Exception("", ex));
            taskRepoMock.Setup(r => r.Save()).Throws(dbEx);

            // Act
            Should.Throw<System.Data.Entity.Infrastructure.DbUpdateException>(()=> { controller.Create(inModel); });

            // Assert
            taskRepoMock.VerifyAll();
            callbackTask.ShouldNotBeNull();
            callbackTask.ParentId.ShouldBe(1);
            callbackTask.Name.ShouldBe("a1");
            controller.ModelState.Count.ShouldBe(0);
        }

        [TestMethod]
        public void Create_Post_ErrorNotHandledType()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var inModel = new TaskCreateModel { ParentId = 1, Name = "a1" };
            Task callbackTask = null;
            taskRepoMock.Setup(r => r.Insert(It.IsAny<Task>())).Callback((Task t) => { callbackTask = t; });
            taskRepoMock.Setup(r => r.Save()).Throws(new NotSupportedException());

            // Act
            Should.Throw<NotSupportedException>(() => { controller.Create(inModel); });

            // Assert
            taskRepoMock.VerifyAll();
            callbackTask.ShouldNotBeNull();
            callbackTask.ParentId.ShouldBe(1);
            callbackTask.Name.ShouldBe("a1");
            controller.ModelState.Count.ShouldBe(0);
        }

        [TestMethod]
        public void Edit_Get()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            taskRepoMock.Setup(r => r.GetByID(1)).Returns(new Task{Id = 1, Name = "a1", ParentId = 2});
            var controller = new HomeController(taskRepoMock.Object);

            // Act
            var result = controller.Edit(1) as ViewResult;

            // Assert
            taskRepoMock.VerifyAll();
            result.ShouldNotBeNull();
            result.Model.ShouldBeOfType<TaskEditModel>();
            var model = (TaskEditModel)result.Model;
            model.Id.ShouldBe(1);
            model.ParentId.ShouldBe(2);
            model.Name.ShouldBe("a1");
        }

        [TestMethod]
        public void Edit_GetNotExist()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            taskRepoMock.Setup(r => r.GetByID(1)).Returns((Task)null);
            var controller = new HomeController(taskRepoMock.Object);

            // Act
            var rez = controller.Edit(1);

            // Assert
            taskRepoMock.VerifyAll();
            rez.ShouldBeOfType<RedirectToRouteResult>();
            var routeResult = (RedirectToRouteResult)rez;
            routeResult.RouteValues.Count.ShouldBe(1);
            routeResult.RouteValues["action"].ShouldBe("Index");
        }

        [TestMethod]
        public void Edit_Post_ValidationFailed()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var inModel = new TaskEditModel { Id = 1, ParentId = 2, Name = "3" };
            controller.ModelState.AddModelError("", "");
            taskRepoMock.Verify(r => r.Save(), Times.Never);

            // Act
            var rez = controller.Edit(inModel);

            // Assert
            rez.ShouldBeOfType<ViewResult>();
            var modelTemp = ((ViewResult)rez).Model;
            modelTemp.ShouldNotBeNull();
            modelTemp.ShouldBeOfType<TaskEditModel>();
            var model = (TaskEditModel)modelTemp;
            model.Id.ShouldBe(1);
            model.ParentId.ShouldBe(2);
            model.Name.ShouldBe("3");
        }

        [TestMethod]
        public void Edit_Post_Success()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var inModel = new TaskEditModel { Id = 1, Name = "a1" };
            var callbackTask = new Task{Id = 1, ParentId = 2, Name = "test"};
            taskRepoMock.Setup(r => r.GetByID(1)).Returns(callbackTask);
            taskRepoMock.Setup(r => r.Save()).Verifiable();

            // Act
            var rez = controller.Edit(inModel);

            // Assert
            taskRepoMock.VerifyAll();
            callbackTask.ShouldNotBeNull();
            callbackTask.Name.ShouldBe("a1");

            rez.ShouldBeOfType<RedirectToRouteResult>();
            var routeResult = (RedirectToRouteResult)rez;
            routeResult.RouteValues.Count.ShouldBe(2);
            routeResult.RouteValues["action"].ShouldBe("Index");
            routeResult.RouteValues["id"].ShouldBe(2);
        }

        [TestMethod]
        public void Edit_Post_ErrorHandled()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var inModel = new TaskEditModel { Id = 1, Name = "a1" };
            var callbackTask = new Task { Id = 1, ParentId = 2, Name = "test" };
            taskRepoMock.Setup(r => r.GetByID(1)).Returns(callbackTask);
            var ex = new Exception("Cannot insert duplicate key row in object 'dbo.Task' with unique index 'IX_ParentId_Name' some other text");
            var dbEx = new System.Data.Entity.Infrastructure.DbUpdateException("", new Exception("", ex));
            taskRepoMock.Setup(r => r.Save()).Throws(dbEx);

            // Act
            var rez = controller.Edit(inModel);

            // Assert
            taskRepoMock.VerifyAll();
            callbackTask.ShouldNotBeNull();
            callbackTask.Name.ShouldBe("a1");
            controller.ModelState.Count.ShouldBe(1);
            controller.ModelState[""].Errors.Single().ErrorMessage.ShouldBe("Task name should be unique on this nesting level");

            rez.ShouldBeOfType<ViewResult>();
            var modelTemp = ((ViewResult)rez).Model;
            modelTemp.ShouldNotBeNull();
            modelTemp.ShouldBeOfType<TaskEditModel>();
            var model = (TaskEditModel)modelTemp;
            model.Id.ShouldBe(1);
            model.ParentId.ShouldBe(2);
            model.Name.ShouldBe("a1");
        }

        [TestMethod]
        public void Edit_Post_ErrorNotHandledText()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var inModel = new TaskEditModel { Id = 1, Name = "a1" };
            var callbackTask = new Task { Id = 1, ParentId = 2, Name = "test" };
            taskRepoMock.Setup(r => r.GetByID(1)).Returns(callbackTask);
            var ex = new Exception("Wrong exception message");
            var dbEx = new System.Data.Entity.Infrastructure.DbUpdateException("", new Exception("", ex));
            taskRepoMock.Setup(r => r.Save()).Throws(dbEx);

            // Act
            Should.Throw<System.Data.Entity.Infrastructure.DbUpdateException>(() => { controller.Edit(inModel); });

            // Assert
            taskRepoMock.VerifyAll();
            callbackTask.ShouldNotBeNull();
            callbackTask.Name.ShouldBe("a1");
            controller.ModelState.Count.ShouldBe(0);
        }

        [TestMethod]
        public void Edit_Post_ErrorNotHandledType()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var inModel = new TaskEditModel { Id = 1, Name = "a1" };
            var callbackTask = new Task { Id = 1, ParentId = 2, Name = "test" };
            taskRepoMock.Setup(r => r.GetByID(1)).Returns(callbackTask);
            taskRepoMock.Setup(r => r.Save()).Throws(new NotSupportedException());

            // Act
            Should.Throw<NotSupportedException>(() => { controller.Edit(inModel); });

            // Assert
            taskRepoMock.VerifyAll();
            callbackTask.ShouldNotBeNull();
            callbackTask.Name.ShouldBe("a1");
            controller.ModelState.Count.ShouldBe(0);
        }

        [TestMethod]
        public void Delete_NotExists()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            taskRepoMock.Setup(r => r.GetByID(1)).Returns((Task)null);
            taskRepoMock.Verify(r => r.Delete(It.IsAny<Task>()), Times.Never);
            taskRepoMock.Verify(r => r.Save(), Times.Never);

            // Act
            var rez = controller.Delete(1);

            // Assert
            taskRepoMock.VerifyAll();
            rez.ShouldBeOfType<RedirectToRouteResult>();
            var routeResult = (RedirectToRouteResult)rez;
            routeResult.RouteValues.Count.ShouldBe(1);
            routeResult.RouteValues["action"].ShouldBe("Index");
        }

        [TestMethod]
        public void Delete_HasSubtasksInCode()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var testTask = new Task{Id=1, ParentId = 2, Name = "a1", Children = new []{new Task()}};
            taskRepoMock.Setup(r => r.GetByID(1)).Returns(testTask);
            taskRepoMock.Verify(r => r.Delete(It.IsAny<Task>()), Times.Never);
            taskRepoMock.Verify(r => r.Save(), Times.Never);

            // Act
            var rez = controller.Delete(1);

            // Assert
            taskRepoMock.VerifyAll();
            rez.ShouldBeOfType<ViewResult>();
            var modelTemp = ((ViewResult)rez).Model;
            modelTemp.ShouldNotBeNull();
            modelTemp.ShouldBeOfType<TaskDeleteModel>();
            var model = (TaskDeleteModel)modelTemp;
            model.Id.ShouldBe(1);
            model.ParentId.ShouldBe(2);
            model.Message.ShouldBe("Can not delete this task, because it contains other tasks.");
        }

        [TestMethod]
        public void Delete_DbErrorHandled()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var testTask = new Task { Id = 1, ParentId = 2, Name = "a1", Children = new List<Task>()};
            Task callbackTask = null;
            taskRepoMock.Setup(r => r.GetByID(1)).Returns(testTask);
            taskRepoMock.Setup(r => r.Delete(It.IsAny<Task>())).Callback((Task task) => { callbackTask = task; });
            var ex = new Exception("The DELETE statement conflicted with the SAME TABLE REFERENCE constraint some other text");
            var dbEx = new System.Data.Entity.Infrastructure.DbUpdateException("", new Exception("", ex));
            taskRepoMock.Setup(r => r.Save()).Throws(dbEx);

            // Act
            var rez = controller.Delete(1);

            // Assert
            taskRepoMock.VerifyAll();
            callbackTask.ShouldBe(testTask);
            rez.ShouldBeOfType<ViewResult>();
            var modelTemp = ((ViewResult)rez).Model;
            modelTemp.ShouldNotBeNull();
            modelTemp.ShouldBeOfType<TaskDeleteModel>();
            var model = (TaskDeleteModel)modelTemp;
            model.Id.ShouldBe(1);
            model.ParentId.ShouldBe(2);
            model.Message.ShouldBe("Can not delete this task, because it contains other tasks.");
        }

        [TestMethod]
        public void Delete_DbErrorNotHandledText()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var testTask = new Task { Id = 1, ParentId = 2, Name = "a1", Children = new List<Task>() };
            Task callbackTask = null;
            taskRepoMock.Setup(r => r.GetByID(1)).Returns(testTask);
            taskRepoMock.Setup(r => r.Delete(It.IsAny<Task>())).Callback((Task task) => { callbackTask = task; });
            var ex = new Exception("Wrong exception message");
            var dbEx = new System.Data.Entity.Infrastructure.DbUpdateException("", new Exception("", ex));
            taskRepoMock.Setup(r => r.Save()).Throws(dbEx);

            // Act
            Should.Throw<System.Data.Entity.Infrastructure.DbUpdateException>(() => { controller.Delete(1); });

            // Assert
            taskRepoMock.VerifyAll();
            callbackTask.ShouldBe(testTask);
        }

        [TestMethod]
        public void Delete_DbErrorNotHandledType()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var testTask = new Task { Id = 1, ParentId = 2, Name = "a1", Children = new List<Task>() };
            Task callbackTask = null;
            taskRepoMock.Setup(r => r.GetByID(1)).Returns(testTask);
            taskRepoMock.Setup(r => r.Delete(It.IsAny<Task>())).Callback((Task task) => { callbackTask = task; });
            taskRepoMock.Setup(r => r.Save()).Throws(new NotSupportedException());

            // Act
            Should.Throw<NotSupportedException>(() => { controller.Delete(1); });

            // Assert
            taskRepoMock.VerifyAll();
            callbackTask.ShouldBe(testTask);
        }

        [TestMethod]
        public void Delete_Success()
        {
            // Arrange
            var taskRepoMock = new Mock<IRepository<Task>>();
            var controller = new HomeController(taskRepoMock.Object);
            var testTask = new Task { Id = 1, ParentId = 2, Name = "a1", Children = new List<Task>()};
            taskRepoMock.Setup(r => r.GetByID(1)).Returns(testTask);

            // Act
            var rez = controller.Delete(1);

            // Assert
            taskRepoMock.VerifyAll();
            rez.ShouldBeOfType<RedirectToRouteResult>();
            var routeResult = (RedirectToRouteResult)rez;
            routeResult.RouteValues.Count.ShouldBe(2);
            routeResult.RouteValues["action"].ShouldBe("Index");
            routeResult.RouteValues["id"].ShouldBe(2);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController(null);

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            result.ShouldNotBeNull();
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController(null);

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            result.ShouldNotBeNull();
        }
    }
}
