import React, { useState } from 'react';

const SyncForm = () => {
    const [units, setUnits] = useState([]);

    const handleFileChange = (event) => {
        const file = event.target.files[0];

        if (!file) return;
        console.log("Файл выбран:", file.name);

        const reader = new FileReader();
        
        reader.onload = (e) => {
            const fileContent = e.target.result;
            const lines = fileContent.split('\n');
            const unitList = [];

            lines.forEach((line) => {
                let [id, parentId, name, status] = line.split(' ');

                if (!parentId) {
                    parentId = null;
                } else {
                    parentId = parentId.trim();
                }
                
                if (id && name && status) {
                    unitList.push({
                        id: id.trim(),
                        parentId,
                        name: name.trim(),
                        status: status.trim(),
                    });
                }
            });

            setUnits(unitList);
            console.log(unitList);
        };

        reader.readAsText(file);
    };

    const fetchUnits = async () => {
        console.log(units);
        const url = 'http://localhost:5195/api/units/sync';
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(units),
        })
            .then((response) => {
                if (response.status === 200) {
                    window.location.reload();
                }
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
