using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class StoredValue<T>
{
	private T value;
	public T Value
	{
		get => value;
		set
		{
			//bool valueChanged = false;

			// Если значение поменялось
			// сохраняем новое значение
			// и инициируем событие OnValueChanged
			//if (value is Array)
   //         {
			//	Array curArray = (this.value as Array);
			//	Array newArray = (value as Array);
			//	for(int i = 0; i < curArray.Length; i++)
   //             {
			//		UnityEngine.Debug.Log(curArray.GetValue(i) + "  " + newArray.GetValue(i));
			//		if (curArray.GetValue(i).Equals(newArray.GetValue(i)) == false)
   //                 {
			//			valueChanged = true;
			//			break;
   //                 }
   //             }
			//}
			if (!this.value.Equals(value))
			{
                this.value = value;
                SaveLoadSystem.Instance.Save();
                OnValueChanged?.Invoke(value);
            }

			//if(valueChanged == true)
			//         {
			//	this.value = value;
			//	SaveLoadSystem.Instance.Save();
			//	OnValueChanged?.Invoke(value);
			//         }
		}
	}

	public event Action<T> OnValueChanged;

	public StoredValue()
	{
		try
		{
			value = Activator.CreateInstance<T>();
		}
		catch (MissingMethodException)
		{
			value = default(T);
		}
	}

	public StoredValue(T value)
	{
		this.value = value;
	}
}