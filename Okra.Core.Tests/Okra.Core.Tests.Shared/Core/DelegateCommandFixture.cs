using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Core;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Okra.Tests.Core
{
    [TestClass]
    public class DelegateCommandFixture
    {
        // *** Constructor Tests ***

        [TestMethod]
        public void Constructor_ReturnsCommand_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler();
            DelegateCommand command = new DelegateCommand(handler.Execute);

            Assert.IsNotNull(command);
        }

        [TestMethod]
        public void Constructor_ReturnsCommand_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute);

            Assert.IsNotNull(command);
        }

        [TestMethod]
        public void Constructor_ReturnsCommand_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute);

            Assert.IsNotNull(command);
        }

        [TestMethod]
        public void Constructor_ReturnsCommandWithCanExecute_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler();
            DelegateCommand command = new DelegateCommand(handler.Execute, handler.CanExecute);

            Assert.IsNotNull(command);
        }

        [TestMethod]
        public void Constructor_ReturnsCommandWithCanExecute_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);

            Assert.IsNotNull(command);
        }

        [TestMethod]
        public void Constructor_ReturnsCommandWithCanExecute_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            Assert.IsNotNull(command);
        }

        [TestMethod]
        public void Constructor_Exception_ExecuteMethodIsNull_NonGeneric()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DelegateCommand(null));
        }

        [TestMethod]
        public void Constructor_Exception_ExecuteMethodIsNull_GenericClass()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DelegateCommand<MockArgumentClass>(null));
        }

        [TestMethod]
        public void Constructor_Exception_ExecuteMethodIsNull_GenericStruct()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DelegateCommand<int>(null));
        }

        [TestMethod]
        public void Constructor_Exception_CanExecuteMethodIsNull_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler();
            Assert.ThrowsException<ArgumentNullException>(() => new DelegateCommand(handler.Execute, null));
        }

        [TestMethod]
        public void Constructor_Exception_CanExecuteMethodIsNull_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            Assert.ThrowsException<ArgumentNullException>(() => new DelegateCommand<MockArgumentClass>(handler.Execute, null));
        }

        [TestMethod]
        public void Constructor_Exception_CanExecuteMethodIsNull_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            Assert.ThrowsException<ArgumentNullException>(() => new DelegateCommand<int>(handler.Execute, null));
        }

        // *** Method Tests ***

        [TestMethod]
        public void Execute_CallsExecuteMethod_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler();
            DelegateCommand command = new DelegateCommand(handler.Execute);

            Assert.AreEqual(0, handler.ExecuteCallCount);

            command.Execute(new MockArgumentClass());

            Assert.AreEqual(1, handler.ExecuteCallCount);
        }

        [TestMethod]
        public void Execute_CallsExecuteMethod_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute);

            Assert.AreEqual(0, handler.ExecuteCallCount);

            command.Execute(new MockArgumentClass());

            Assert.AreEqual(1, handler.ExecuteCallCount);
        }

        [TestMethod]
        public void Execute_CallsExecuteMethod_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute);

            Assert.AreEqual(0, handler.ExecuteCallCount);

            command.Execute(42);

            Assert.AreEqual(1, handler.ExecuteCallCount);
        }

        [TestMethod]
        public void Execute_PassesArgument_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute);
            MockArgumentClass argument = new MockArgumentClass();

            command.Execute(argument);

            CollectionAssert.AreEqual(new[] { argument }, (ICollection)handler.ArgumentList);
        }

        [TestMethod]
        public void Execute_PassesArgument_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute);

            command.Execute(42);

            CollectionAssert.AreEqual(new[] { 42 }, (ICollection)handler.ArgumentList);
        }

        [TestMethod]
        public void CanExecute_CallsCanExecuteMethod_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler();
            DelegateCommand command = new DelegateCommand(handler.Execute, handler.CanExecute);

            Assert.AreEqual(0, handler.CanExecuteCallCount);

            command.CanExecute(new MockArgumentClass());

            Assert.AreEqual(1, handler.CanExecuteCallCount);
        }

        [TestMethod]
        public void CanExecute_CallsCanExecuteMethod_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);

            Assert.AreEqual(0, handler.CanExecuteCallCount);

            command.CanExecute(new MockArgumentClass());

            Assert.AreEqual(1, handler.CanExecuteCallCount);
        }

        [TestMethod]
        public void CanExecute_CallsCanExecuteMethod_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            Assert.AreEqual(0, handler.CanExecuteCallCount);

            command.CanExecute(42);

            Assert.AreEqual(1, handler.CanExecuteCallCount);
        }

        [TestMethod]
        public void CanExecute_PassesArgument_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);
            MockArgumentClass argument = new MockArgumentClass();

            command.CanExecute(argument);

            CollectionAssert.AreEqual(new[] { argument }, (ICollection)handler.ArgumentList);
        }

        [TestMethod]
        public void CanExecute_PassesArgument_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            command.CanExecute(42);

            CollectionAssert.AreEqual(new[] { 42 }, (ICollection)handler.ArgumentList);
        }

        [TestMethod]
        public void CanExecute_WillReturnTrue_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler() { CanExecuteValue = true };
            DelegateCommand command = new DelegateCommand(handler.Execute, handler.CanExecute);

            Assert.IsTrue(command.CanExecute(new MockArgumentClass()));
        }

        [TestMethod]
        public void CanExecute_WillReturnTrue_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>() { CanExecuteValue = true };
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);

            Assert.IsTrue(command.CanExecute(new MockArgumentClass()));
        }

        [TestMethod]
        public void CanExecute_WillReturnTrue_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>() { CanExecuteValue = true };
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            Assert.IsTrue(command.CanExecute(42));
        }

        [TestMethod]
        public void CanExecute_WillReturnFalse_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler() { CanExecuteValue = false };
            DelegateCommand command = new DelegateCommand(handler.Execute, handler.CanExecute);

            Assert.IsFalse(command.CanExecute(new MockArgumentClass()));
        }

        [TestMethod]
        public void CanExecute_WillReturnFalse_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>() { CanExecuteValue = false };
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);

            Assert.IsFalse(command.CanExecute(new MockArgumentClass()));
        }

        [TestMethod]
        public void CanExecute_WillReturnFalse_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>() { CanExecuteValue = false };
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            Assert.IsFalse(command.CanExecute(42));
        }

        [TestMethod]
        public void CanExecute_ReturnsFalseIfIncorrectType_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>() { CanExecuteValue = true };
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);

            Assert.IsFalse(command.CanExecute(42));
        }

        [TestMethod]
        public void CanExecute_ReturnsFalseIfIncorrectType_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>() { CanExecuteValue = true };
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            Assert.IsFalse(command.CanExecute(new MockArgumentClass()));
        }

        [TestMethod]
        public void CanExecute_ReturnsTrueIfNullParameter_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>() { CanExecuteValue = true };
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);

            Assert.IsTrue(command.CanExecute(null));
        }

        [TestMethod]
        public void CanExecute_ReturnsFalseIfNullParameter_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>() { CanExecuteValue = true };
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            Assert.IsFalse(command.CanExecute(null));
        }

        [TestMethod]
        public void NotifyCanExecuteChanged_FiresCanExecuteChangedEvent()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);

            int canExecuteChangedCount = 0;
            command.CanExecuteChanged += (sender, e) =>
                {
                    canExecuteChangedCount++;
                };

            command.NotifyCanExecuteChanged();

            Assert.AreEqual(1, canExecuteChangedCount);
        }

        // *** Private Sub-classes ***

        private class MockArgumentClass
        {
        }

        private class MockCommandHandler
        {
            // *** Properties ***

            public int ExecuteCallCount { get; set; }
            public int CanExecuteCallCount { get; set; }
            public bool CanExecuteValue { get; set; }

            // *** Methods ***

            public void Execute()
            {
                ExecuteCallCount++;
            }

            public bool CanExecute()
            {
                CanExecuteCallCount++;
                return CanExecuteValue;
            }
        }

        private class MockCommandHandler<T>
        {
            // *** Properties ***

            public int ExecuteCallCount { get; set; }
            public int CanExecuteCallCount { get; set; }
            public bool CanExecuteValue { get; set; }
            public IList<T> ArgumentList = new List<T>();

            // *** Methods ***

            public void Execute(T argument)
            {
                ExecuteCallCount++;
                ArgumentList.Add(argument);
            }

            public bool CanExecute(T argument)
            {
                CanExecuteCallCount++;
                ArgumentList.Add(argument);
                return CanExecuteValue;
            }
        }
    }
}
