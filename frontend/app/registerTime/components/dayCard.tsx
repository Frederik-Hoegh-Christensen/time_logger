import React from 'react';
import styles from '../registerTime.module.css';

interface DayCardProps {
  date: Date;
  hoursWorked?: number;
  minutesWorked?: number;
  isSelected: boolean;
  onSelect: (date: Date) => void;
}

const DayCard: React.FC<DayCardProps> = ({ date, isSelected, onSelect }) => {
  return (
    <div 
      className={styles.cardContainer}
      onClick={() => onSelect(date)}
      style={{
        backgroundColor: isSelected ? 'lightblue' : 'white',
        cursor: 'pointer',
      }}
    >
      <div className={styles.date}>
        <strong>{date.toDateString()}</strong>
      </div>
      
    </div>
  );
};

export default DayCard;
