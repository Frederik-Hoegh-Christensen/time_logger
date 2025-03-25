import React from "react";
import { Link } from "react-router";
import styles from './Sidebar.module.css'
const SideBar: React.FC = () => {


    return (
        <div className={styles.sidebar}>
            <nav>
            <Link to='/projectOverview' className={styles.navLink}>
                <span>Project overview</span>
            </Link>
            <Link to="/timeRegistration" className={styles.navLink}>
                <span>Time Registration</span>
            </Link>
            </nav>
        </div>
    );
};

export default SideBar;
