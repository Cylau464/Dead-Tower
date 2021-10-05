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
			if (this.value == null || !this.value.Equals(value))
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