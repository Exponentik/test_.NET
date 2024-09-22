import React, { useState } from 'react';


const SyncForm = () => {
    const [units, setUnits] = useState([]);

    const handleFileChange = (event) => {
        const file = event.target.files[0];

        if (!file) return;
        console.log("Файл выбран:", file.name);

        const reader = new FileReader();
        
        reader.onload = (e) => {
            const fileContent = e.target.result; // Переменная должна быть объявлена здесь
            console.log(fileContent);
            const lines = fileContent.split('\n');
            const unitList = [];

            lines.forEach((line) => {
                const [id, parentId, name, status] = line.split(' ');
                if (id && parentId && name && status) {
                    unitList.push({
                        id: id.trim(),
                        parentId: parentId.trim(),
                        name: name.trim(),
                        status: status.trim(),
                    });
                }
            });

            // Устанавливаем список юнитов в состояние
            setUnits(unitList);
            console.log(unitList);
        };

        reader.readAsText(file); // Этот вызов должен быть здесь
    };

    const fetchUnits = async () => {
        console.log(units);
        const url = 'http://localhost:5195/api/units/sync'; // Ваш URL
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(units),
        })
            .then((response) => response.json())
            .then((data) => {
                console.log('Успешный ответ:', data);
            })
            .catch((error) => {
                console.error('Ошибка:', error);
            });
    };

    return (
        <div>
            <button onClick={fetchUnits}>
                Синхронизировать
            </button>
            <button onClick={() => document.getElementById('fileInput').click()}>
                Загрузить файл
            </button>
            <input
                id="fileInput"  
                type="file"
                style={{ display: 'none' }}
                accept=".txt"
                onChange={handleFileChange}
            />
        </div>
    );
};

export default SyncForm;
