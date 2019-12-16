package md5c8ea674ae9c22359b488e63fe31848d0;


public class BadgeView
	extends android.widget.TextView
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Plugin.Badge.Droid.BadgeView, Plugin.Badge.Droid", BadgeView.class, __md_methods);
	}


	public BadgeView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == BadgeView.class)
			mono.android.TypeManager.Activate ("Plugin.Badge.Droid.BadgeView, Plugin.Badge.Droid", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public BadgeView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == BadgeView.class)
			mono.android.TypeManager.Activate ("Plugin.Badge.Droid.BadgeView, Plugin.Badge.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public BadgeView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == BadgeView.class)
			mono.android.TypeManager.Activate ("Plugin.Badge.Droid.BadgeView, Plugin.Badge.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public BadgeView (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == BadgeView.class)
			mono.android.TypeManager.Activate ("Plugin.Badge.Droid.BadgeView, Plugin.Badge.Droid", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
