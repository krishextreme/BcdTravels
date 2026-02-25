using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Ledger.FileGenerator // Replace with the actual namespace if known
{
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [DebuggerNonUserCode]
    [CompilerGenerated]
    internal class Resources
    {
        private static ResourceManager resourceManager;
        private static CultureInfo resourceCulture;

        internal Resources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (resourceManager == null)
                {
                    resourceManager = new ResourceManager("YourNamespace.Properties.Resources", typeof(Resources).Assembly);
                }
                return resourceManager;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get => resourceCulture;
            set => resourceCulture = value;
        }

        internal static Bitmap all_colours
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject(nameof(all_colours), resourceCulture);
            }
        }

        internal static Bitmap AmericanExpress
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject(nameof(AmericanExpress), resourceCulture);
            }
        }

        internal static Bitmap Calendar
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject(nameof(Calendar), resourceCulture);
            }
        }

        internal static Bitmap CreditCard
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject(nameof(CreditCard), resourceCulture);
            }
        }

        internal static Bitmap Crossmark
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject(nameof(Crossmark), resourceCulture);
            }
        }

        internal static Bitmap DinersClub
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject(nameof(DinersClub), resourceCulture);
            }
        }

        internal static Bitmap Discover
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject(nameof(Discover), resourceCulture);
            }
        }

        internal static Bitmap HalfMoon
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject(nameof(HalfMoon), resourceCulture);
            }
        }

        internal static Bitmap JCB
        {
            get => (Bitmap)ResourceManager.GetObject(nameof(JCB), resourceCulture);
        }

        internal static Bitmap Maestro
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject(nameof(Maestro), resourceCulture);
            }
        }

        internal static Bitmap MasterCard
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject(nameof(MasterCard), resourceCulture);
            }
        }

        internal static Bitmap RuPay
        {
            get => (Bitmap)ResourceManager.GetObject(nameof(RuPay), resourceCulture);
        }

        internal static Bitmap SunLight
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject(nameof(SunLight), resourceCulture);
            }
        }

        internal static Bitmap UATP
        {
            get => (Bitmap)ResourceManager.GetObject(nameof(UATP), resourceCulture);
        }

        internal static Bitmap UnionPay
        {
            get
            {
                return (Bitmap)ResourceManager.GetObject(nameof(UnionPay), resourceCulture);
            }
        }

        internal static Bitmap Visa
        {
            get => (Bitmap)ResourceManager.GetObject(nameof(Visa), resourceCulture);
        }

        internal static Bitmap Yes
        {
            get => (Bitmap)ResourceManager.GetObject(nameof(Yes), resourceCulture);
        }
    }
}
