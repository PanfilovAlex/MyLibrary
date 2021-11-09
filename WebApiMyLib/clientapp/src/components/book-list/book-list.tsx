import { useEffect, useState } from 'react';
import { Book } from '../../models/book';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import { Pagination, PaginationItem } from '@mui/material';
import { Link, useLocation } from 'react-router-dom';

export function BookList() {
    const [books, setBooks] = useState<Book[]>([]);

    const location = useLocation();
    const query = new URLSearchParams(location.search);
    const page = parseInt(query.get('page') || '1', 10);
    const itemsPerPage = 2;
    const pageCount = Math.ceil(books.length / itemsPerPage);
    const skippedItems = (page - 1) * itemsPerPage;

    useEffect(() => {
        fetch('api/book')
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                setBooks(data as Book[]);
            });
    }, []);

    // const handleItemClick = useCallback(() => {

    // }, []);

    return (
        <>
            <List>
                {
                    books.slice(skippedItems, skippedItems + itemsPerPage).map((book) => {
                        return (
                            <ListItem
                                key={book.bookId}>

                                <ListItemText
                                    primary={book.title}
                                    secondary={book.autor}
                                />
                            </ListItem>
                        );
                    })
                }
            </List>

            {pageCount > 1 && (
                <Pagination
                    page={page}
                    count={pageCount}
                    renderItem={(item) => (
                        <PaginationItem
                            component={Link}
                            to={`/books${item.page === 1 ? '' : `?page=${item.page}`}`}
                            {...item} />
                    )}
                />
            )}

        </>
    );
}
