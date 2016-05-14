using System.Reflection;
using System.Runtime.InteropServices;
using Xamarin.Forms;
using Xamarin.Forms.Platform.PerspexDesktop;


// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Xamarin.Forms.Platform.Perspex.Desktop")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Xamarin.Forms.Platform.Perspex.Desktop")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("5d79dc0b-29b2-442a-a9de-fa7cfa5da887")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: Dependency(typeof(WindowsResourcesProvider))]
[assembly: Dependency(typeof(WindowsSerializer))]


// Views

[assembly: ExportRenderer(typeof(Layout), typeof(LayoutRenderer))]
[assembly: ExportRenderer(typeof(BoxView), typeof(BoxViewRenderer))]
[assembly: ExportRenderer(typeof(Image), typeof(ImageRenderer))]
[assembly: ExportRenderer(typeof(Label), typeof(LabelRenderer))]
[assembly: ExportRenderer(typeof(Button), typeof(ButtonRenderer))]
[assembly: ExportRenderer(typeof(ListView), typeof(ListViewRenderer))]
[assembly: ExportRenderer(typeof(ScrollView), typeof(ScrollViewRenderer))]
[assembly: ExportRenderer(typeof(ProgressBar), typeof(ProgressBarRenderer))]
[assembly: ExportRenderer(typeof(Slider), typeof(SliderRenderer))]
//[assembly: ExportRenderer(typeof(Switch), typeof(SwitchRenderer))]
//[assembly: ExportRenderer(typeof(WebView), typeof(WebViewRenderer))]
[assembly: ExportRenderer(typeof(Frame), typeof(FrameRenderer))]
//[assembly: ExportRenderer(typeof(ActivityIndicator), typeof(ActivityIndicatorRenderer))]
[assembly: ExportRenderer(typeof(Editor), typeof(EditorRenderer))]
//[assembly: ExportRenderer(typeof(Picker), typeof(PickerRenderer))]
//[assembly: ExportRenderer(typeof(TimePicker), typeof(TimePickerRenderer))]
//[assembly: ExportRenderer(typeof(DatePicker), typeof(DatePickerRenderer))]
//[assembly: ExportRenderer(typeof(Stepper), typeof(StepperRenderer))]
[assembly: ExportRenderer(typeof(Entry), typeof(EntryRenderer))]
//[assembly: ExportRenderer(typeof(TableView), typeof(TableViewRenderer))]
//[assembly: ExportRenderer(typeof(NativeViewWrapper), typeof(NativeViewWrapperRenderer))]

// Pages

[assembly: ExportRenderer(typeof(Page), typeof(PageRenderer))]

// Cells

[assembly: ExportCell(typeof(Cell), typeof(TextCellRenderer))]
[assembly: ExportCell(typeof(ImageCell), typeof(ImageCellRenderer))]
[assembly: ExportCell(typeof(EntryCell), typeof(EntryCellRenderer))]
[assembly: ExportCell(typeof(SwitchCell), typeof(SwitchCellRenderer))]
[assembly: ExportCell(typeof(ViewCell), typeof(ViewCellRenderer))]
