using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Ledger.ScrollBar
{
    // Base textbox with image support
    public class BaseImageTextBox : UserControl
    {
        protected TextBox contentTextField;
        protected string actualText = "";

        private Image image;
        private Point imageExpand = new Point(2, 2);
        private Size textOffset = new Size(0, 0);
        private Color placeholderColor = Color.Gray;
        private string placeholderText = "";
        private Color backgroundColor = Color.White;

        public event EventHandler ContentChanged;

        public BaseImageTextBox()
        {
            contentTextField = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Location = new Point(10, 10),
                Size = new Size(200, 20)
            };

            contentTextField.TextChanged += ContentTextField_TextChanged;
            Controls.Add(contentTextField);

            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void ContentTextField_TextChanged(object sender, EventArgs e)
        {
            actualText = contentTextField.Text;
            ContentChanged?.Invoke(this, e);
            Invalidate();
        }

        public string Content
        {
            get => contentTextField.Text;
            set
            {
                contentTextField.Text = value;
                actualText = value;
            }
        }

        public Image Image
        {
            get => image;
            set
            {
                image = value;
                Invalidate();
            }
        }

        public Point ImageExpand
        {
            get => imageExpand;
            set
            {
                imageExpand = value;
                Invalidate();
            }
        }

        public Size TextOffset
        {
            get => textOffset;
            set
            {
                textOffset = value;
                Invalidate();
            }
        }

        public Color PlaceholderColor
        {
            get => placeholderColor;
            set
            {
                placeholderColor = value;
                Invalidate();
            }
        }

        public string PlaceholderText
        {
            get => placeholderText;
            set
            {
                placeholderText = value;
                Invalidate();
            }
        }

        public new Color BackColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                base.BackColor = value;
                contentTextField.BackColor = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw background
            using (SolidBrush brush = new SolidBrush(backgroundColor))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }

            // Draw image if present
            if (image != null)
            {
                Rectangle imgRect = new Rectangle(
                    5,
                    (Height - image.Height) / 2,
                    image.Width + imageExpand.X,
                    image.Height + imageExpand.Y
                );
                e.Graphics.DrawImage(image, imgRect);
            }

            // Draw placeholder if text is empty
            if (string.IsNullOrEmpty(contentTextField.Text) && !string.IsNullOrEmpty(placeholderText))
            {
                using (SolidBrush brush = new SolidBrush(placeholderColor))
                {
                    e.Graphics.DrawString(placeholderText, Font, brush,
                        contentTextField.Location.X + textOffset.Width,
                        contentTextField.Location.Y + textOffset.Height);
                }
            }
        }
    }

    // Resources for card images
    public static class CreditCardResources
    {
        public static Image credit_card => CreateCardIcon("CC");
        public static Image visa => CreateCardIcon("VISA");
        public static Image mastercard => CreateCardIcon("MC");
        public static Image americanexpress => CreateCardIcon("AMEX");
        public static Image discover => CreateCardIcon("DISC");
        public static Image jcb => CreateCardIcon("JCB");
        public static Image maestro => CreateCardIcon("MAE");
        public static Image dinersclub => CreateCardIcon("DIN");
        public static Image unionpay => CreateCardIcon("UP");
        public static Image rupay => CreateCardIcon("RuP");

        private static Bitmap CreateCardIcon(string text)
        {
            Bitmap bmp = new Bitmap(32, 20);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);

                // Draw card outline
                using (Pen pen = new Pen(Color.Gray, 1))
                {
                    g.DrawRectangle(pen, 1, 1, 29, 17);
                }

                // Draw text
                using (Font font = new Font("Arial", 6, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.Gray))
                {
                    StringFormat format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(text, font, brush, new RectangleF(0, 0, 32, 20), format);
                }
            }
            return bmp;
        }
    }

    [DefaultEvent(nameof(IsCardValidChanged))]
    public class CuiTextBoxCreditCardNumber : BaseImageTextBox
    {
        private bool _isCardValid;
        private IContainer components;

        public event EventHandler IsCardValidChanged;

        public CuiTextBoxCreditCardNumber()
        {
            InitializeComponent();
            ContentChanged += CuiTextBoxCreditCardNumber_ContentChanged;
        }

        private void FormatCreditCardSoFar()
        {
            int selectionStart = contentTextField.SelectionStart;

            // Keep only digits, cap to 19
            string cleanedDigits = new string(
                actualText
                    .Where(char.IsDigit)
                    .Take(19)
                    .ToArray());

            // Group digits 4-4-4-4-...
            string formatted = string.Join(
                " ",
                Enumerable.Range(0, (cleanedDigits.Length + 3) / 4)
                          .Select(i =>
                          {
                              int start = i * 4;
                              int len = Math.Min(4, cleanedDigits.Length - start);
                              return cleanedDigits.Substring(start, len);
                          }));

            if (actualText == formatted)
                return;

            // Preserve caret position
            int spacesBeforeCaret = actualText
                .Substring(0, Math.Min(selectionStart, actualText.Length))
                .Count(c => c == ' ');

            int caretInDigits = selectionStart - spacesBeforeCaret;

            int newCaret = 0;
            int digitsConsumed = 0;

            foreach (string group in formatted.Split(new[] { ' ' }, StringSplitOptions.None))
            {
                if (digitsConsumed + group.Length >= caretInDigits)
                {
                    newCaret += caretInDigits - digitsConsumed;
                    break;
                }

                newCaret += group.Length + 1;
                digitsConsumed += group.Length;
            }

            contentTextField.Text = formatted;
            contentTextField.SelectionStart = Math.Min(newCaret, contentTextField.Text.Length);
        }

        private void CuiTextBoxCreditCardNumber_ContentChanged(object sender, EventArgs e)
        {
            FormatCreditCardSoFar();

            switch (DetectCardType(Content))
            {
                case CardType.Unknown:
                    Image = CreditCardResources.credit_card;
                    break;
                case CardType.Visa:
                    Image = CreditCardResources.visa;
                    break;
                case CardType.MasterCard:
                    Image = CreditCardResources.mastercard;
                    break;
                case CardType.AmericanExpress:
                    Image = CreditCardResources.americanexpress;
                    break;
                case CardType.Discover:
                    Image = CreditCardResources.discover;
                    break;
                case CardType.JCB:
                    Image = CreditCardResources.jcb;
                    break;
                case CardType.Maestro:
                    Image = CreditCardResources.maestro;
                    break;
                case CardType.DinersClub:
                    Image = CreditCardResources.dinersclub;
                    break;
                case CardType.UnionPay:
                    Image = CreditCardResources.unionpay;
                    break;
                case CardType.RuPay:
                    Image = CreditCardResources.rupay;
                    break;
            }
        }

        public bool IsCardValid => _isCardValid && DetectCardType(actualText) != CardType.Unknown;

        private bool IsCardValidState
        {
            get
            {
                string digits = actualText.Replace(" ", "");
                bool validLength = digits.Length > 11 && digits.Length < 20;

                if (_isCardValid != validLength)
                {
                    _isCardValid = validLength;
                    IsCardValidChanged?.Invoke(this, null);
                    return _isCardValid;
                }

                _isCardValid = validLength;
                return _isCardValid;
            }
        }

        public CardType DetectCardType(string cardNumber)
        {
            cardNumber = (cardNumber ?? string.Empty).Replace(" ", "");

            if (string.IsNullOrWhiteSpace(cardNumber) || !IsCardValidState)
                return CardType.Unknown;

            // Ensure we have enough digits
            if (cardNumber.Length < 6)
                return CardType.Unknown;

            int iin2 = int.Parse(cardNumber.Substring(0, 2));
            int iin3 = int.Parse(cardNumber.Substring(0, 3));
            int iin4 = int.Parse(cardNumber.Substring(0, 4));
            int iin6 = int.Parse(cardNumber.Substring(0, 6));

            if (cardNumber.StartsWith("4"))
                return CardType.Visa;

            if (iin2 >= 51 && iin2 <= 55 || iin4 >= 2221 && iin4 <= 2720)
                return CardType.MasterCard;

            if (cardNumber.StartsWith("34") || cardNumber.StartsWith("37"))
                return CardType.AmericanExpress;

            if (cardNumber.StartsWith("6011") ||
                cardNumber.StartsWith("65") ||
                iin6 >= 622126 && iin6 <= 622925 ||
                iin3 >= 644 && iin3 <= 649)
                return CardType.Discover;

            if (iin4 >= 3528 && iin4 <= 3589)
                return CardType.JCB;

            if (iin4 == 5018 || iin4 == 5020 || iin4 == 5038 || iin4 == 5893 ||
                iin4 == 6304 || iin4 == 6759 || iin4 == 6761 || iin4 == 6762 || iin4 == 6763)
                return CardType.Maestro;

            if (iin2 == 36 || iin2 == 38 || iin2 == 39)
                return CardType.DinersClub;

            if (cardNumber.StartsWith("62"))
                return CardType.UnionPay;

            if (iin6 >= 508500 && iin6 <= 508999 ||
                iin6 >= 606985 && iin6 <= 607984 ||
                iin6 >= 608001 && iin6 <= 608500 ||
                iin6 >= 652150 && iin6 <= 653149)
                return CardType.RuPay;

            return cardNumber.StartsWith("1") ? CardType.UATP : CardType.Unknown;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();

            SuspendLayout();

            contentTextField.BackColor = Color.FromArgb(16, 16, 16);
            contentTextField.Location = new Point(41, 15);
            contentTextField.Size = new Size(184, 15);

            AutoScaleDimensions = new SizeF(8f, 16f);
            AutoScaleMode = AutoScaleMode.Font;

            Image = CreditCardResources.credit_card;
            ImageExpand = new Point(2, 2);
            Name = "cuiTextBoxCreditCardNumber";

            base.Padding = new System.Windows.Forms.Padding(41, 15, 41, 0);
            PlaceholderColor = Color.Gray;
            TextOffset = new Size(26, 0);

            ResumeLayout(false);
            PerformLayout();
        }

        public enum CardType
        {
            Unknown,
            Visa,
            MasterCard,
            AmericanExpress,
            Discover,
            JCB,
            Maestro,
            DinersClub,
            UnionPay,
            RuPay,
            UATP,
        }
    }
}