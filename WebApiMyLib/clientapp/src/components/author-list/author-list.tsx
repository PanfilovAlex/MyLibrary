import { useCallback, useEffect, useState } from 'react';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import { Button, IconButton, Pagination, PaginationItem } from '@mui/material';
import { Link, useLocation } from 'react-router-dom';
import { Author } from '../../models/author';
import { getFullName } from '../../selectors/author-selectors';
import DeleteIcon from '@mui/icons-material/Delete';
import { AuthorEditor } from './author-editor/author-editor';

// TODO: think on how to generalize list components (book, author, category)
export function AuthorList() {
    const pageSize = 10;
    const [totalCount, setTotalCount] = useState<number>(0);
    const [authors, setAuthors] = useState<Author[]>([]);
    const [authorForEditing, setAuthorForEditing] = useState<Author>();
    const [isEditorOpened, setIsEditorOpened] = useState<boolean>(false);

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
                setAuthors(data as Author[]);
                setTotalCount(data.totalCount);
            });
    }, [pageNumber]);

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

    const handleItemClick = (author: Author) => {
        setAuthorForEditing(author);
        setIsEditorOpened(true);
    };

    const handleCreateButtonClick = () => {
        setIsEditorOpened(true);
    };

    const handleEditorClose = () => {
        setAuthorForEditing(undefined);
        setIsEditorOpened(false);
    };

    const handleEditorSubmit = (author: Author) => {
        setAuthors((current) => {
            if (current.find((a) => a.id === author.id)) {
                return [...current.filter((a) => a.id !== author.id), author];
            }
            return [...current, author];
        });
        setIsEditorOpened(false);
    };

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
                                    onClick={() => handleItemClick(author)}
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
                <ListItem>
                    <Button
                        variant="text"
                        onClick={handleCreateButtonClick}>
                        Add Author
                    </Button>
                </ListItem>
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

            {isEditorOpened && (
                <AuthorEditor
                    author={authorForEditing}
                    onClose={handleEditorClose}
                    onSubmit={handleEditorSubmit} />
            )}
        </>
    );
}
