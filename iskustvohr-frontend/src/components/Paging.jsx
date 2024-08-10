import React from 'react';

export default function Paging({ pageCount, currentPage, setCurrentPage }) {
    const handlePageChange = (page) => {
        setCurrentPage(page);
    };

    const renderPageNumbers = () => {
        return Array.from({ length: pageCount }, (_, index) => index + 1).map((page) => (
            <li key={page} className={`page-item ${currentPage === page ? 'active' : ''}`}>
                <button className="page-link" onClick={() => handlePageChange(page)}>
                    {page}
                </button>
            </li>
        ));
    };

    return (
        <div className="d-flex justify-content-center">
            <nav>
                <ul className="pagination">
                    <li className={`page-item ${currentPage === 1 ? 'disabled' : ''}`}>
                        <button className="page-link" onClick={() => handlePageChange(currentPage - 1)}>
                            Prethodna
                        </button>
                    </li>
                    {renderPageNumbers()}
                    <li className={`page-item ${currentPage === pageCount ? 'disabled' : ''}`}>
                        <button className="page-link" onClick={() => handlePageChange(currentPage + 1)}>
                            Sljedeća
                        </button>
                    </li>
                </ul>
            </nav>
        </div>
    );
}
