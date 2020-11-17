using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace VisitorPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Visitor Pattern!");

            Form form = Get();

            IVisitor visitor = new HtmlVisitor("Hello");

            form.Accept(visitor);

            string html = visitor.Output;

            System.IO.File.WriteAllText("index.html", html);
        }

        public static Form Get()
        {
            Form form = new Form
            {
                Name = "/forms/customers",
                Title = "Design Patterns",

                Body = new Collection<ControlBase>
                {

                    new LabelControl { Caption = "Person", Name = "lblName" },
                    new TextBoxControl { Caption = "FirstName", Name = "txtFirstName", Value = "John"},
                    new CheckBoxControl { Caption = "IsAdult", Name = "chkIsAdult", Value = true },
                    new ButtonControl {  Caption = "Submit", Name = "btnSubmit", ImageSource = "save.png" },
                }

            };

            return form;
        }
    }

    #region Models

    public class Form
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public ICollection<ControlBase> Body { get; set; }

        public void Accept(IVisitor visitor)
        {
            foreach (ControlBase control in Body)
            {
                control.Accept(visitor);
            }
        }
    }

    public class Control
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public ControlType Type { get; set; }
        public string Value { get; set; }
        public string ImageSource { get; set; }
    }

    public enum ControlType
    {
        Label,
        TextBox,
        Checkbox,
        Button
    }

    public abstract class ControlBase
    {
        public string Name { get; set; }
        public string Caption { get; set; }

        public abstract void Accept(IVisitor visitor);

        //public override void Accept(IVisitor visitor)
        //{
        //    visitor.Visit(this);
        //}
    }

    public class LabelControl : ControlBase
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class TextBoxControl : ControlBase
    {
        public string Value { get; set; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class CheckBoxControl : ControlBase
    {
        public bool Value { get; set; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class ButtonControl : ControlBase
    {
        public string ImageSource { get; set; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class RadioButtonControl : ControlBase
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }


    // Abstract Visitor

    public interface IVisitor
    {
        void Visit(LabelControl control);
        void Visit(TextBoxControl control);
        void Visit(CheckBoxControl control);
        void Visit(ButtonControl control);
        void Visit(RadioButtonControl control);
        string Output { get; }
    }

    // Concrete Visitor
    public class HtmlVisitor : IVisitor
    {
        private readonly StringBuilder builder = new StringBuilder();

        public string Output
        {
            get
            {
                BeginDocument("hello");
                EndDocument();

                return builder.ToString();
            }
        }

        public HtmlVisitor(string title)
        {
            BeginDocument(title);
        }

        private void BeginDocument(string title)
        {
            builder.AppendLine("<html>");

            builder.AppendLine($"<title>{title}</title>");

            builder.AppendLine("<body>");
        }

        private void EndDocument()
        {
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");

        }

        public void Visit(LabelControl control)
        {
            builder.AppendLine($"<span>{control.Caption}</span>");
        }

        public void Visit(TextBoxControl control)
        {
            builder.AppendLine($"<span>{control.Caption}</span><input type='text' value='{control.Value}'></input>");
        }

        public void Visit(CheckBoxControl control)
        {
            builder.AppendLine($"<span>{control.Caption}</span><input type='checkbox' value='{control.Value}'></input>");
        }

        public void Visit(ButtonControl control)
        {
            builder.AppendLine($"<button><img src='{control.ImageSource}'/>{control.Caption}</button>");
        }

        public void Visit(RadioButtonControl control)
        {
            throw new NotImplementedException();
        }
    }

    // Concrete Visitor

    public class MarkdownVisitor : IVisitor
    {
        public string Output => throw new NotImplementedException();

        public void Visit(LabelControl control)
        {
            throw new NotImplementedException();
        }

        public void Visit(TextBoxControl control)
        {
            throw new NotImplementedException();
        }

        public void Visit(CheckBoxControl control)
        {
            throw new NotImplementedException();
        }

        public void Visit(ButtonControl control)
        {
            throw new NotImplementedException();
        }

        public void Visit(RadioButtonControl control)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

}
