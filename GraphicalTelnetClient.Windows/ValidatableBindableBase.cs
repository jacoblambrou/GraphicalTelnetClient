using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GraphicalTelnetClient.Windows
{
    public class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo, INotifyPropertyChanged
    {
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            else
                return null;
        }

        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }

        protected override bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            bool changed = base.SetProperty(ref member, val, propertyName);
            ValidateProperty(propertyName, val);
            return changed;
        }

        public void ValidateProperty<T>(string propertyName, T value)
        {
            var results = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(this);
            context.MemberName = propertyName;

            try
            {
                Validator.TryValidateProperty(value, context, results);
            }
            catch (Exception ex)
            {
                Validator.TryValidateProperty(ex.Message, context, results);
            }
            finally
            {
                EnumerateErrors(propertyName, results);

                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        private void EnumerateErrors(string propertyName, List<ValidationResult> results)
        {
            if (results.Any())
            {
                _errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                _errors.Remove(propertyName);
            }
        }
    }
}
