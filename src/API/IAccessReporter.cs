namespace Rhenus.Core.API
{
	/// <summary>
	/// Used to report to the IAccessCoordinator when access is requested to a shared object. 
	/// Methods must be called in the context of an active transaction, or provide a transaction 
	/// in which the context of the request took place. The latter option is primarily provided for
	/// services to report accesses that were only detected during the prepare phase when the 
	/// transaction is no longer active.
	/// 
	/// All methods of IAccessReporter take an object identifier as a parameter. This parameter 
	/// must implement Object.Equals() and Object.HashCode(). To make the resulting detail provided
	/// to the profiler as useful as possible, the identifier should have a meaningful 
	/// Object.ToString() method. Other than this the identifier may be any arbitrary instance, 
	/// including the requested object itself, as long as it uniquely identifies the object across 
	/// transactions.
	/// 
	/// For the reportObjectAccess methods, access should be reported as early as possible. 
	/// In particular, if actually resolving or retrieving the object could fail, or incur any 
	/// significant expense, the report should be made first. This is to ensure that access is 
	/// always noted, and has the chance to abort the transaction before any unneeded processing is 
	/// done. For instance, in the case of the DataService, before a name binding is 
	/// resolved in the getBinding method, the requested access to that bound object should
	/// be reported.
	/// 
	/// If the implementation of the reportObjectAccess methods detect a conflict and wish 
	/// to cause the calling transaction to fail, they will abort the transaction and then throw a 
	/// TransactionAbortedException.
	/// <param name="T">the type of the identifier used to identify accessed objects</param>
	/// </summary>
	// TODO: make sure the object type's equals and hashCode methods work in a similar way as they do in Java 
	public interface IAccessReporter<T>
	{
		/// <summary>
		/// Reports to the coordinator that object access has been requested in the context of the 
		/// current transaction. The requested object is shared, and may be the cause of conflict.
		/// </summary>
		/// <param name='objID'>
		/// An identifier for the object being accessed.
		/// </param>
		/// <param name='type'>
		/// The AccessType being requested.
		/// </param>
		/// <exception cref="TransactionNotActiveException">if not called in the context of an 
		/// active tranaction</exception>
		/// <exception cref="TransactionAbortedException">if access failed due to a conflict
		/// </exception>
		void ReportObjectAccess (T objID, AccessType type);

		/// <summary>
		/// Reports to the coordinator that object access has been requested in the context of the 
		/// provided transaction. The requested object is shared, and may be the cause of conflict.
		/// </summary>
		/// <param name='txn'>
		/// The transaction in which the provided objId was accessed.
		/// </param>
		/// <param name='objID'>
		/// An identifier for the object being accessed.
		/// </param>
		/// <param name='type'>
		/// The AccessType being requested
		/// </param>
		/// <exception cref="System.ArgumentException">if the provided transaction is invalid, has 
		/// already committed, or is otherwise unknown to the IAccessCoordinator</exception>
		/// <exception cref="TransactionAbortedException">if access failed due to a conflict
		/// </exception>
		void ReportObjectAccess (ITransaction txn, T objID, AccessType type);

		/// <summary>
		/// Reports to the coordinator that an object access with the provided description has been
		/// requested in the context of the current transaction. The requested object is shared, 
		/// and may be the cause of conflict. See setObjectDescription for more details about 
		/// description.
		/// </summary>
		/// <param name='objId'>
		/// An identifier for the object being accessed.
		/// </param>
		/// <param name='type'>
		/// The AccessType being requested.
		/// </param>
		/// <param name='description'>
		/// An arbitrary object that contains a description of the object being accessed.
		/// </param>
		/// <exception cref="TransactionNotActiveException">if not called in the context of an 
		/// active transaction</exception>
		/// <exception cref="TransactionAbortedExcpetion">if access failed due to a conflict
		/// </exception>
		void reportObjectAccess (T objId, AccessType type, object description);

		/// <summary>
		/// Reports to the coordinator that an object access with the provided description has been
		/// requested in the context of the provided transaction. The requested object is shared, 
		/// and may be the cause of conflict. See setObjectDescription for more details about 
		/// description.
		/// </summary>
		/// <param name='txn'>
		/// the transaction in which the provided objId was accessed.
		/// </param>
		/// <param name='objId'>
		/// An identifier for the object being accessed.
		/// </param>
		/// <param name='type'>
		/// The AccessType being requested.
		/// </param>
		/// <param name='description'>
		/// An arbitrary object that contains a description of the object being accessed.
		/// </param>
		/// <exception cref="System.ArgumentException">if the provided transaction is invalid, has 
		/// already committed, or is otherwise unknown to the IAccessCoordinator</exception>
		/// <exception cref="TransactionAbortedException">if access failed due to a conflict
		/// </exception>
		void reportObjectAccess (ITransaction txn, T objId, AccessType type, object description);

		/// <summary>
		/// In the current transaction, associates the given object with some description that 
		/// should have a meaningful toString method. This description will be available in
		/// the profiling data, and is useful when displaying details about a given accessed 
		/// object. The intent is that an arbitrary description can be included with an object, but 
		/// that the description is not accessed unless a ProfileListener finds it useful 
		/// to do so. At that point the description's toString method may be called, or the 
		/// object itself might be cast to some known type to extract more detail about the 
		/// accessed object.
		/// 
		/// Note that this may be called before the associated object is actually accessed, and 
		/// therefore before reportObjectAccess is called for the given objId. Use
		/// of this method is optional, and only used to provide additional detail for profiling 
		/// and debugging.
		/// 
		/// If a description has already been set for the identified object, or if the provided 
		/// description is <c>null</c>, then no change is made to the description of the object.
		/// </summary>
		/// <param name='objId'>
		/// The identifier for the associated object
		/// </param>
		/// <param name='description'>
		/// An arbitrary Object that contains a description of the objId being accessed
		/// </param>
		/// <exception cref="TransactionNotActiveException">if not called in the context of an 
		/// active transaction</exception>
		void setObjectDescription (T objId, object description);

		/// <summary>
		/// In the provided transaction, associates the given object with some description that 
		/// should have a meaningful toString method. This description will be available in the 
		/// profiling data, and is useful when displaying details about a given accessed object. 
		/// The intent is that an arbitrary description can be included with an object, but that 
		/// the description is not accessed unless a ProfileListener finds it useful to do so. At 
		/// that point the description's toString method may be called, or the object itself might 
		/// be cast to some known type to extract more detail about the accessed object.
		/// 
		/// Note that this may be called before the associated object is actually accessed, and 
		/// therefore before reportObjectAccess is called for the given objId. Use of this 
		/// method is optional, and only used to provide additional detail for profiling and 
		/// debugging.
		/// 
		/// If a description has already been set for the identified object, or if the provided 
		/// description is <c>null</c>, then no change is made to the description of the object.
		/// </summary>
		/// <param name='txn'>
		/// The transaction in which the provided objId was accessed.
		/// </param>
		/// <param name='objId'>
		/// The identifier for the associated object.
		/// </param>
		/// <param name='description'>
		/// An arbitrary Object that contains a description of the objId being accessed.
		/// </param>
		/// <exception cref="System.ArgumentException">if the provided transaction is invalid, has 
		/// already committed, or is otherwise unknown to the IAccessCoordinator</exception>
		void setObjectDescription (ITransaction txn, T objId, object description);
	}

	/// The type of access requested
	public enum AccessType
	{
		/// <summary>
		/// The object is accessed, but not modified.
		/// </summary>
		READ,
		/// <summary>
		/// The object is being modified.
		/// </summary>
		WRITE
	}
}

