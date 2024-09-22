import React, { useEffect, useState } from "react";
import { fetchUnits } from "../services/UnitService";
import UnitItem from "./UnitItem";
const UnitList = () => {
    const [units, setUnits] = useState([]);

    const fetchData = async () => {
        const units = await fetchUnits();
        setUnits(units);
    };

    useEffect(() => {
        fetchData(); // Первоначальный вызов для загрузки данных

        const interval = setInterval(fetchData, 3000); // Обновление данных каждые 3 секунды

        return () => clearInterval(interval); // Очистка интервала при размонтировании компонента
    }, []);

    if (units.length === 0) {
        return <h1 style={{ textAlign: 'center' }}>Департаменты не найдены</h1>;
    }

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

    // Рекурсивная функция для отображения дерева
    const renderTree = (nodes) => {
        return nodes.map(node => (
            <div key={node.id} style={{ marginLeft: '20px' }}>
                <UnitItem className="unit" unit={node} />
                {node.children.length > 0 && renderTree(node.children)}
            </div>
        ));
    };

    const unitTree = buildTree(units);

    return (
        <div>
            {renderTree(unitTree)}
        </div>
    );
};

export default UnitList;
