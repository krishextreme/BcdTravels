

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Ledger.BitUI
{
    [ToolboxBitmap(typeof(Button))]
    public class FormDragComponent : Component
    {
        private Control dragHandleControl;          // was targetControl
        private Point previousMousePosition;        // unchanged meaning
        private Form draggedForm;                   // was parentForm

        public FormDragComponent(IContainer container)
        {
            container.Add((IComponent)this);
        }

        public Control TargetControl
        {
            get => this.dragHandleControl;
            set
            {
                if (this.dragHandleControl != null)
                {
                    this.dragHandleControl.MouseDown -= new MouseEventHandler(this.OnMouseDown);
                    this.dragHandleControl.MouseMove -= new MouseEventHandler(this.OnMouseMove);
                }

                this.dragHandleControl = value;

                if (this.dragHandleControl == null)
                    return;

                this.dragHandleControl.MouseDown += new MouseEventHandler(this.OnMouseDown);
                this.dragHandleControl.MouseMove += new MouseEventHandler(this.OnMouseMove);
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            this.previousMousePosition = Cursor.Position;
            this.draggedForm = this.dragHandleControl?.FindForm();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || this.draggedForm == null)
                return;

            Point position = Cursor.Position;
            int deltaX = position.X - this.previousMousePosition.X;
            int deltaY = position.Y - this.previousMousePosition.Y;

            this.draggedForm.Left += deltaX;
            this.draggedForm.Top += deltaY;

            this.previousMousePosition = position;
        }
    }
}
