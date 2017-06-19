using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorExample
{
    public abstract class BaseViewModel : System.ComponentModel.INotifyPropertyChanged, System.ComponentModel.IDataErrorInfo
    {
        #region Notes
        #endregion
        #region Events
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Fields
        /// <summary>
        /// This is a dictionary of property names, and any rules that go with that property.
        /// </summary>
        private Dictionary<string, PropertyRule> ruleMap = new Dictionary<string, PropertyRule>();
        private bool _HasError;
        private bool _HasChanges;
        #endregion
        #region Properties
        public string this[string columnName]
        {
            get
            {
                if (ruleMap.ContainsKey(columnName))
                {
                    ruleMap[columnName].Update();
                    return ruleMap[columnName].Error;
                }
                return null;
            }
        }
        /// <summary>
        /// WPF seems to not use the Error call, theoretically it is used to invalidate an object.
        /// </summary>
        public string Error { get; set; }
        public bool HasError { get { return _HasError; } }
        public bool HasChanges { get { return _HasChanges; } }
        #endregion
        #region Constructors
        public BaseViewModel()
        {
            AddValidationRules();
        }
        #endregion
        #region Voids
        abstract public void AddValidationRules();
        public void Validate()
        {
            _HasError = false;
            StringBuilder errors = new StringBuilder();
            foreach (PropertyRule pr in ruleMap.Values)
            {
                pr.Update();
                if (pr.HasError)
                {
                    errors.AppendLine(pr.Error);
                    _HasError = true;
                }
            }
            if (HasError)
            {
                Error = errors.ToString();
            }
            else
            {
                Error = null;
            }
        }
        abstract public void Save();
        protected void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            _HasChanges = true;
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        protected void AddRule(string PropertyName, Func<bool> ruleDelegate, string errorMessage)
        {
            if (ruleMap.ContainsKey(PropertyName))
            {
                ruleMap[PropertyName].AddRule(ruleDelegate, errorMessage);
            }
            else
            {
                ruleMap.Add(PropertyName, new PropertyRule(ruleDelegate, errorMessage));
            }
        }
        //public void CopyFrom(BaseViewModel inputViewModel)
        //{
        //    if (this.GetType() == inputViewModel.GetType())
        //    {
        //        System.Reflection.FieldInfo[] fi = inputViewModel.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //        foreach(System.Reflection.FieldInfo info in fi)
        //        {
        //            System.Reflection.FieldInfo myfi = this.GetType().GetField(info.Name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //            myfi.SetValue(this, Convert.ChangeType(info.GetValue(inputViewModel),info.FieldType));
        //        }
        //    }else
        //    {
        //        //error message out.
        //    }
        //}
        #endregion
        #region Functions
        #endregion
        #region InternalClasses
        protected class PropertyRule
        {
            private List<Rule> _rules = new List<Rule>();
            internal bool HasError { get; set; }
            //internal bool IsDirty { get; set; }
            internal string Error { get; set; }
            internal PropertyRule(Func<bool> rule, string errormessage)
            {
                _rules.Add(new Rule(rule, errormessage));
            }
            internal void AddRule(Func<bool> rule, string errormessage)
            {
                _rules.Add(new Rule(rule, errormessage));
            }
            internal void Update()
            {
                Error = null;
                HasError = false;
                try
                {
                    foreach (Rule r in _rules)
                    {
                        if (!r.expression())
                        {
                            Error += r.message + "\n";
                            HasError = true;
                        }
                    }
                    if (HasError)
                    {
                        Error = Error.TrimEnd(new Char[] { '\n' });
                    }
                }
                catch (Exception e)
                {
                    Error = e.Message;
                    HasError = true;
                }
            }
            private class Rule
            {
                public readonly Func<bool> expression;
                public readonly string message;
                internal Rule(Func<bool> expr, string msg)
                {
                    expression = expr;
                    message = msg;
                }
            }
        }
        #endregion
    }
}
