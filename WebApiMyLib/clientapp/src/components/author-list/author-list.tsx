import { useEffect, useState } from 'react';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import { Pagination, PaginationItem } from '@mui/material';
import { Link, useLocation } from 'react-router-dom';
import { Author } from '../../models/author';

// TODO: think on how to generalize list components (book, author, category)
export function AuthorList() {
    const [authors, setAuthors] = useState<Author[]>([]);

    const location = useLocation();
    const query = new URLSearchParams(location.search);
    const page = parseInt(query.get('page') || '1', 10);
    const pageCount = Math.ceil(authors.length / 2);
    const itemsPerPage = 2;
    const skippedItems = (page - 1) * itemsPerPage;

    useEffect(() => {
        fetch('api/author')
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                setAuthors(data as Author[]);
            });
    }, []);

    return (
        <>
            <List>
                {
                    authors.slice(skippedItems, skippedItems + itemsPerPage).map((author) => {
                        return (
                            <ListItem
                                key={author.authorId}>

                                <ListItemText
                                    primary={author.firstName + author.lastName}
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
                            to={`/authors${item.page === 1 ? '' : `?page=${item.page}`}`}
                            {...item} />
                    )}
                />
            )}

        </>
    );
}
