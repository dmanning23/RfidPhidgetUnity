using UnityEngine;
using System.Collections;
using Phidgets; //Needed for the RFID class and the PhidgetException class
using Phidgets.Events; //Needed for the phidget event handling classes

public class RfidReader : MonoBehaviour
{
	#region Fields
	
	/// <summary>
	/// Singleton access to this object
	/// </summary>
	public static RfidReader GetInstance;

	/// <summary>
	/// GUIText for displaying currently connected car
	/// </summary>
	public GameObject resultDisplay;

	/// <summary>
	/// Declare an RFID object
	/// </summary>
	RFID rfid;

	string MessageText;

	#endregion //Fields

	#region Properties

	#endregion //Properties

	#region Methods

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	public void Awake()
	{
		GetInstance = this;
	}

	/// <summary>
	/// Start is called just before any of the Update methods is called the first time.
	/// </summary>
	public void Start()
	{
		//initialize our Phidgets RFID reader and hook the event handlers
		rfid = new RFID();
		rfid.Attach += new AttachEventHandler(rfid_Attach);
		rfid.Detach += new DetachEventHandler(rfid_Detach);
		rfid.Error += new ErrorEventHandler(rfid_Error);

		rfid.Tag += new TagEventHandler(rfid_Tag);
		rfid.TagLost += new TagEventHandler(rfid_TagLost);
		rfid.open();

		MessageText = "Waiting for device attach...";
	}

	/// <summary>
	/// Update is called once per frame
	/// </summary>
	public void Update()
	{
		resultDisplay.guiText.text = MessageText;
	}

	/// <summary>
	/// attach event handler...display the serial number of the attached RFID phidget
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void rfid_Attach(object sender, AttachEventArgs e)
	{
		//Get the toast message component
		rfid.Antenna = true;
		rfid.LED = true;

		//pop up a message
		MessageText = string.Format("RfidReader {0} attached!", e.Device.SerialNumber.ToString());
	}

	/// <summary>
	/// detach event handler...display the serial number of the detached RFID phidget
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void rfid_Detach(object sender, DetachEventArgs e)
	{
		MessageText = string.Format("RfidReader {0} detached!", e.Device.SerialNumber.ToString());
	}

	/// <summary>
	/// Error event handler...display the error description string
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void rfid_Error(object sender, ErrorEventArgs e)
	{
		MessageText = string.Format(e.Description);
	}

	/// <summary>
	/// Print the tag code of the scanned tag
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void rfid_Tag(object sender, TagEventArgs e)
	{
		MessageText = string.Format("Tag {0} scanned", e.Tag);
	}

	/// <summary>
	/// print the tag code for the tag that was just lost
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void rfid_TagLost(object sender, TagEventArgs e)
	{
		MessageText = string.Format("Tag {0} lost", e.Tag);
	}

	#endregion //Methods
}
