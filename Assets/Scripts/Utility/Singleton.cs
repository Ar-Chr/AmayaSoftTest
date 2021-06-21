using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
	public static T Instance { get; private set; }

	protected virtual void Awake() {
		if ( Instance != null && Instance != this ) {
			Debug.LogError($"[Singleton] Trying to instantiate a second instance of a singleton class: " + typeof(T).Name);
		}
		Instance = (T)this;
	}

	protected void OnDestroy() {
		if ( Instance == this ) {
			Instance = null;
		}
	}
}
