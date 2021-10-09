using System;
using System.Collections.Generic;
using System.Collections;
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
            // Если значение поменялось
            // сохраняем новое значение
            // и инициируем событие OnValueChanged
			bool valueChanged = false;

            if (value is Array || this.value == null || !this.value.Equals(value))
            {
				valueChanged = true;
			}

			if(valueChanged == true)
            {
                this.value = value;
				SaveValue();
            }
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

	public void SaveValue()
	{
		SaveLoadSystem.Instance.Save();
		OnValueChanged?.Invoke(value);
	}
}