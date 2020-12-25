using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UserInterfaceTest.Commands.Base;

namespace UserInterfaceTest.Commands
{
    /// <summary>
    /// Provides an implementation of the <see cref="ICommand"/> interface.
    /// </summary>
    public class RelayCommand : CommandBase
    {
        private readonly Action execute;

        private readonly Func<bool> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        public RelayCommand(Action execute) : this(execute, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        /// <param name="canExecute">The predicate to check whether the function can be executed.</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>True if command can be executed; False otherwise.</returns>
        public override bool CanExecute()
        {
            return this.canExecute == null || this.canExecute();
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        public override void Execute()
        {
            this.execute();
        }
    }

    /// <summary>
    /// Provides an implementation of the <see cref="ICommand"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the command parameter.</typeparam>
    public class RelayCommand<T> : CommandBase<T>
    {
        private readonly Action<T> execute;

        private readonly Predicate<T> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        /// <param name="canExecute">The predicate to check whether the function can be executed.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Gets a value indicating whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        /// <returns>True if command can be executed; False otherwise.</returns>
        [DebuggerStepThrough]
        public override bool CanExecute(T parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public override void Execute(T parameter)
        {
            this.execute(parameter);
        }
    }
}
