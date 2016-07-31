using System;
using System.Collections.Generic;
using Okra.Core;
using Xunit;

namespace Okra.Tests.Core
{
    public class DelegateCommandFixture
    {
        // *** Constructor Tests ***

        [Fact]
        public void Constructor_ReturnsCommand_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler();
            DelegateCommand command = new DelegateCommand(handler.Execute);

            Assert.NotNull(command);
        }

        [Fact]
        public void Constructor_ReturnsCommand_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute);

            Assert.NotNull(command);
        }

        [Fact]
        public void Constructor_ReturnsCommand_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute);

            Assert.NotNull(command);
        }

        [Fact]
        public void Constructor_ReturnsCommandWithCanExecute_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler();
            DelegateCommand command = new DelegateCommand(handler.Execute, handler.CanExecute);

            Assert.NotNull(command);
        }

        [Fact]
        public void Constructor_ReturnsCommandWithCanExecute_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);

            Assert.NotNull(command);
        }

        [Fact]
        public void Constructor_ReturnsCommandWithCanExecute_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            Assert.NotNull(command);
        }

        [Fact]
        public void Constructor_Exception_ExecuteMethodIsNull_NonGeneric()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new DelegateCommand(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: execute", e.Message);
            Assert.Equal("execute", e.ParamName);
        }

        [Fact]
        public void Constructor_Exception_ExecuteMethodIsNull_GenericClass()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new DelegateCommand<MockArgumentClass>(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: execute", e.Message);
            Assert.Equal("execute", e.ParamName);
        }

        [Fact]
        public void Constructor_Exception_ExecuteMethodIsNull_GenericStruct()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new DelegateCommand<int>(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: execute", e.Message);
            Assert.Equal("execute", e.ParamName);
        }

        [Fact]
        public void Constructor_Exception_CanExecuteMethodIsNull_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler();
            var e = Assert.Throws<ArgumentNullException>(() => new DelegateCommand(handler.Execute, null));

            Assert.Equal("Value cannot be null.\r\nParameter name: canExecute", e.Message);
            Assert.Equal("canExecute", e.ParamName);
        }

        [Fact]
        public void Constructor_Exception_CanExecuteMethodIsNull_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            var e = Assert.Throws<ArgumentNullException>(() => new DelegateCommand<MockArgumentClass>(handler.Execute, null));

            Assert.Equal("Value cannot be null.\r\nParameter name: canExecute", e.Message);
            Assert.Equal("canExecute", e.ParamName);
        }

        [Fact]
        public void Constructor_Exception_CanExecuteMethodIsNull_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            var e = Assert.Throws<ArgumentNullException>(() => new DelegateCommand<int>(handler.Execute, null));

            Assert.Equal("Value cannot be null.\r\nParameter name: canExecute", e.Message);
            Assert.Equal("canExecute", e.ParamName);
        }

        // *** Method Tests ***

        [Fact]
        public void Execute_CallsExecuteMethod_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler();
            DelegateCommand command = new DelegateCommand(handler.Execute);

            Assert.Equal(0, handler.ExecuteCallCount);

            command.Execute(new MockArgumentClass());

            Assert.Equal(1, handler.ExecuteCallCount);
        }

        [Fact]
        public void Execute_CallsExecuteMethod_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute);

            Assert.Equal(0, handler.ExecuteCallCount);

            command.Execute(new MockArgumentClass());

            Assert.Equal(1, handler.ExecuteCallCount);
        }

        [Fact]
        public void Execute_CallsExecuteMethod_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute);

            Assert.Equal(0, handler.ExecuteCallCount);

            command.Execute(42);

            Assert.Equal(1, handler.ExecuteCallCount);
        }

        [Fact]
        public void Execute_PassesArgument_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute);
            MockArgumentClass argument = new MockArgumentClass();

            command.Execute(argument);

            Assert.Equal<MockArgumentClass>(new[] { argument }, handler.ArgumentList);
        }

        [Fact]
        public void Execute_PassesArgument_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute);

            command.Execute(42);

            Assert.Equal<int>(new[] { 42 }, handler.ArgumentList);
        }

        [Fact]
        public void CanExecute_CallsCanExecuteMethod_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler();
            DelegateCommand command = new DelegateCommand(handler.Execute, handler.CanExecute);

            Assert.Equal(0, handler.CanExecuteCallCount);

            command.CanExecute(new MockArgumentClass());

            Assert.Equal(1, handler.CanExecuteCallCount);
        }

        [Fact]
        public void CanExecute_CallsCanExecuteMethod_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);

            Assert.Equal(0, handler.CanExecuteCallCount);

            command.CanExecute(new MockArgumentClass());

            Assert.Equal(1, handler.CanExecuteCallCount);
        }

        [Fact]
        public void CanExecute_CallsCanExecuteMethod_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            Assert.Equal(0, handler.CanExecuteCallCount);

            command.CanExecute(42);

            Assert.Equal(1, handler.CanExecuteCallCount);
        }

        [Fact]
        public void CanExecute_PassesArgument_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>();
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);
            MockArgumentClass argument = new MockArgumentClass();

            command.CanExecute(argument);

            Assert.Equal<MockArgumentClass>(new[] { argument }, handler.ArgumentList);
        }

        [Fact]
        public void CanExecute_PassesArgument_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>();
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            command.CanExecute(42);

            Assert.Equal<int>(new[] { 42 }, handler.ArgumentList);
        }

        [Fact]
        public void CanExecute_WillReturnTrue_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler() { CanExecuteValue = true };
            DelegateCommand command = new DelegateCommand(handler.Execute, handler.CanExecute);

            Assert.True(command.CanExecute(new MockArgumentClass()));
        }

        [Fact]
        public void CanExecute_WillReturnTrue_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>() { CanExecuteValue = true };
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);

            Assert.True(command.CanExecute(new MockArgumentClass()));
        }

        [Fact]
        public void CanExecute_WillReturnTrue_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>() { CanExecuteValue = true };
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            Assert.True(command.CanExecute(42));
        }

        [Fact]
        public void CanExecute_WillReturnFalse_NonGeneric()
        {
            MockCommandHandler handler = new MockCommandHandler() { CanExecuteValue = false };
            DelegateCommand command = new DelegateCommand(handler.Execute, handler.CanExecute);

            Assert.False(command.CanExecute(new MockArgumentClass()));
        }

        [Fact]
        public void CanExecute_WillReturnFalse_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>() { CanExecuteValue = false };
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);

            Assert.False(command.CanExecute(new MockArgumentClass()));
        }

        [Fact]
        public void CanExecute_WillReturnFalse_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>() { CanExecuteValue = false };
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            Assert.False(command.CanExecute(42));
        }

        [Fact]
        public void CanExecute_ReturnsFalseIfIncorrectType_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>() { CanExecuteValue = true };
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);

            Assert.False(command.CanExecute(42));
        }

        [Fact]
        public void CanExecute_ReturnsFalseIfIncorrectType_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>() { CanExecuteValue = true };
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            Assert.False(command.CanExecute(new MockArgumentClass()));
        }

        [Fact]
        public void CanExecute_ReturnsTrueIfNullParameter_GenericClass()
        {
            MockCommandHandler<MockArgumentClass> handler = new MockCommandHandler<MockArgumentClass>() { CanExecuteValue = true };
            DelegateCommand<MockArgumentClass> command = new DelegateCommand<MockArgumentClass>(handler.Execute, handler.CanExecute);

            Assert.True(command.CanExecute(null));
        }

        [Fact]
        public void CanExecute_ReturnsFalseIfNullParameter_GenericStruct()
        {
            MockCommandHandler<int> handler = new MockCommandHandler<int>() { CanExecuteValue = true };
            DelegateCommand<int> command = new DelegateCommand<int>(handler.Execute, handler.CanExecute);

            Assert.False(command.CanExecute(null));
        }

        [Fact]
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

            Assert.Equal(1, canExecuteChangedCount);
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
