import React, { useEffect, useState } from "react";
import { fetchUnits } from "../services/UnitService";
import UnitItem from "./UnitItem";

const UnitList = () => {
    const [units, setUnits] = useState([]);
    const [filter, setFilter] = useState(""); // Состояние для фильтра

    const fetchData = async () => {
        const fetchedUnits = await fetchUnits();
        setUnits(fetchedUnits || []);
    };

    useEffect(() => {
        fetchData(); // Первоначальный вызов для загрузки данных

        const interval = setInterval(fetchData, 3000); // Обновление данных каждые 3 секунды

        return () => clearInterval(interval); // Очистка интервала при размонтировании компонента
    }, []);

    // Фильтрация по наименованию подразделения
    const filteredUnits = units.filter(unit =>
        unit.name.toLowerCase().includes(filter.toLowerCase())
    );

    // Функция для построения дерева
    const buildTree = (units) => {
        const map = {};
        units.forEach(unit => {
            map[unit.id] = { ...unit, children: [] };
        });
        const tree = [];
        units.forEach(unit => {
            if (unit.parentId) {
                map[unit.parentId].children.push(map[unit.id]);
            } else {
                tree.push(map[unit.id]);
            }
        });
        return tree;
    };

    // Рекурсивная функция для рендеринга дерева
    const renderTree = (nodes) => {
        return nodes.map(node => (
            <div key={node.id} style={{ marginLeft: '20px' }}>
                <UnitItem className="unit" unit={node} />
                {node.children.length > 0 && renderTree(node.children)}
            </div>
        ));
    };

    const unitTree = buildTree(filteredUnits);

    return (
        <div>
            <input
                type="text"
                placeholder="Поиск подразделения..."
                value={filter}
                onChange={(e) => setFilter(e.target.value)}
                style={{ marginBottom: '20px', width: '100%', padding: '8px' }}
            />
            {unitTree.length > 0 ? renderTree(unitTree) : (
                <h1 style={{ textAlign: 'center' }}>Подразделения не найдены</h1>
            )}
        </div>
    );
};

export default UnitList;
