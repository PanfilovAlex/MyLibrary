import { useCallback, useEffect, useState } from 'react';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import { IconButton, Pagination, PaginationItem } from '@mui/material';
import { Link, useLocation } from 'react-router-dom';
import { Author } from '../../models/author';
import { getFullName } from '../../selectors/author-selectors';
import DeleteIcon from '@mui/icons-material/Delete';

// TODO: think on how to generalize list components (book, author, category)
export function AuthorList() {
    const pageSize = 10;
    const [totalCount, setTotalCount] = useState(0);
    const [authors, setAuthors] = useState<Author[]>([]);

    const location = useLocation();
    const query = new URLSearchParams(location.search);
    const pageNumber = parseInt(query.get('pageNumber') || '1', 10);
    const numberOfPages = Math.ceil(totalCount / pageSize);

    useEffect(() => {
        const url = `api/author?pageNumber=${pageNumber}&pageSize=${pageSize}`;
        fetch(url)
            .then((response) => {
                if (response.ok) {
                    return response.json();
                }
                console.error(`error occurred while fetching authors, url: ${url}`);
            })
            .then((data) => {
                setAuthors(data.items as Author[]);
                setTotalCount(data.totalCount);
            });
    }, []);

    const handleDeleteButtonClick = useCallback((id: number) => {
        const url = `api/author/${id}`;
        fetch(url, {
            method: 'DELETE',
        })
            // TODO: should respond with status code
            //   .then((response) => response.json())
            .then((data) => {
                if (data.ok) {
                    setAuthors((current) => {
                        return current.filter((author) => author.id !== id);
                    });
                } else {
                    console.error(`error occurred while deleting author, url: ${url}`);
                }
            });
    }, [setAuthors]);

    return (
        <>
            <List>
                {
                    authors.map((author) => {
                        return (
                            <ListItem
                                key={author.id}>

                                <ListItemText
                                    primary={getFullName(author)}
                                />

                                <IconButton
                                    color="primary"
                                    onClick={() => handleDeleteButtonClick(author.id!)}>
                                    <DeleteIcon />
                                </IconButton>
                            </ListItem>
                        );
                    })
                }
            </List>

            {numberOfPages > 1 && (
                <Pagination
                    page={pageNumber}
                    count={numberOfPages}
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
