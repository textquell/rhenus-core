namespace Rhenus.Core.API
{
	/// <summary>
	/// An interface that provides details of a single object access. Two accessed objects are 
	/// identical if both were reported by an AccessReporter obtained by registering an access 
	/// source with a single AccessCoordinator using the same source name, and the values returned 
	/// by getObjectId() and getAccessType() are equal.
	/// </summary>
	interface IAccessedObject
	{
		/// <summary>
		/// Returns the identifier for the accessed object.
		/// </summary>
		/// <returns>
		/// The identifier for the accessed object.
		/// </returns>
		object GetObjectId ();

		/// <summary>
		/// Returns the type of access requested.
		/// </summary>
		/// <returns>
		/// The AccessType.
		/// </returns>
		AccessType GetAccessType ();

		/// <summary>
		/// Returns the supplied description of the object, if any.
		/// </summary>
		/// <returns>
		/// The associated description, or <c>null</c>
		/// </returns>
		/// <seealso cref="IAccessReporter.setObjectDescription(object, object)"/>
		object GetDescription ();

		/// <summary>
		/// Returns the name of the source that reported this object access.
		/// </summary>
		/// <returns>
		/// The object's source.
		/// </returns>
		string GetSource ();
	}
}